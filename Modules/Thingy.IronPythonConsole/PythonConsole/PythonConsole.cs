// Copyright (c) 2010 Joe Moorhouse

using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using IronPython.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Hosting.Shell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Style = Microsoft.Scripting.Hosting.Shell.Style;

namespace PythonConsoleControl
{
    public delegate void ConsoleInitializedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Custom IronPython console. The command dispacher runs on a separate UI thread from the REPL
    /// and also from the WPF control.
    /// </summary>
    public sealed class PythonConsole : IConsole, IDisposable
    {
        private volatile bool _executing;
        private bool _allowCtrlSpaceAutocompletion;
        private bool _allowFullAutocompletion;
        private bool _consoleInitialized;
        private bool _disableAutocompletionForCallables;
        private int _promptLength;
        private CommandLineHistory _commandLineHistory;
        private ManualResetEvent _disposedEvent;
        private ManualResetEvent _lineReceivedEvent;
        private int _lineReceivedEventIndex;
        private List<string> _previousLines;
        // The index into the waitHandles array where the lineReceivedEvent is stored.
        private AutoResetEvent _statementsExecutionRequestedEvent;

        private CommandLine _commandLine;
        private Dispatcher dispatcher;

        // This is the thread upon which all commands execute unless the dipatcher is overridden.
        private Thread _dispatcherThread;
        private Window _dispatcherWindow;
        private string _prompt;
        private PythonTextEditor _textEditor;
        private WaitHandle[] _waitHandles;

        /// <summary>
        /// Returns true if the user can backspace at the current cursor position.
        /// </summary>
        private bool CanBackspace
        {
            get
            {
                if (_textEditor.SelectionLength > 0) return SelectionIsDeletable;
                else
                {
                    int cursorIndex = _textEditor.Column - _promptLength - 1;
                    return !IsCurrentLineReadOnly && (cursorIndex > 0 || (cursorIndex == 0 && _textEditor.SelectionLength > 0));
                }
            }
        }

        /// <summary>
        /// Returns true if the user can delete at the current cursor position.
        /// </summary>
        private bool CanDelete
        {
            get
            {
                if (_textEditor.SelectionLength > 0) return SelectionIsDeletable;
                else return !IsInReadOnlyRegion;
            }
        }

        /// <summary>
        /// Only the last line in the text editor is not read only.
        /// </summary>
        private bool IsCurrentLineReadOnly
        {
            get { return _textEditor.Line < _textEditor.TotalLines; }
        }

        /// <summary>
        /// Determines whether the current cursor position is in a prompt.
        /// </summary>
        private bool IsInPrompt
        {
            get { return _textEditor.Column - _promptLength - 1 < 0; }
        }

        /// <summary>
        /// Returns true if the cursor is in a readonly text editor region.
        /// </summary>
        private bool IsInReadOnlyRegion
        {
            get { return IsCurrentLineReadOnly || IsInPrompt; }
        }

        private bool SelectionIsDeletable
        {
            get
            {
                return (!_textEditor.SelectionIsMultiline
                    && !IsCurrentLineReadOnly
                    && (_textEditor.SelectionStartColumn - _promptLength - 1 >= 0)
                    && (_textEditor.SelectionEndColumn - _promptLength - 1 >= 0));
            }
        }

        private void DispatcherThreadStartingPoint()
        {
            _dispatcherWindow = new Window();
            dispatcher = _dispatcherWindow.Dispatcher;
            while (true)
            {
                try
                {
                    Dispatcher.Run();
                }
                catch (ThreadAbortException tae)
                {
                    if (tae.ExceptionState is Microsoft.Scripting.KeyboardInterruptException)
                    {
                        Thread.ResetAbort();
                        Executing = false;
                    }
                }
            }
        }

        /// <summary>
        /// Run on the statement execution thread.
        /// </summary>
        private void ExecuteStatements(string scriptText)
        {
            lock (scriptText)
            {
                _textEditor.Write("\r\n");
                ScriptSource scriptSource = _commandLine.ScriptScope.Engine.CreateScriptSourceFromString(scriptText, SourceCodeKind.Statements);
                string error = "";
                try
                {
                    Executing = true;
                    scriptSource.Execute(_commandLine.ScriptScope);
                }
                catch (ThreadAbortException tae)
                {
                    if (tae.ExceptionState is KeyboardInterruptException) Thread.ResetAbort();
                    error = "KeyboardInterrupt" + System.Environment.NewLine;
                }
                catch (SyntaxErrorException exception)
                {
                    ExceptionOperations eo;
                    eo = _commandLine.ScriptScope.Engine.GetService<ExceptionOperations>();
                    error = eo.FormatException(exception);
                }
                catch (Exception exception)
                {
                    ExceptionOperations eo;
                    eo = _commandLine.ScriptScope.Engine.GetService<ExceptionOperations>();
                    error = eo.FormatException(exception) + Environment.NewLine;
                }
                Executing = false;
                if (error != "") _textEditor.Write(error);
                _textEditor.Write(_prompt);
            }
        }

        private string GetLastTextEditorLine()
        {
            return _textEditor.GetLine(_textEditor.TotalLines - 1);
        }

        /// <summary>
        /// The home position is at the start of the line after the prompt.
        /// </summary>
        private void MoveToHomePosition()
        {
            _textEditor.Line = _textEditor.TotalLines;
            _textEditor.Column = _promptLength + 1;
        }

        /// <summary>
        /// Shows the next command line in the command line history.
        /// </summary>
        private void MoveToNextCommandLine()
        {
            _textEditor.Line = _textEditor.TotalLines;
            if (_commandLineHistory.MoveNext())
            {
                ReplaceCurrentLineTextAfterPrompt(_commandLineHistory.Current);
            }
        }

        /// <summary>
        /// Shows the previous command line in the command line history.
        /// </summary>
        private void MoveToPreviousCommandLine()
        {
            if (_commandLineHistory.MovePrevious())
            {
                ReplaceCurrentLineTextAfterPrompt(_commandLineHistory.Current);
            }
        }

        /// <summary>
        /// Move cursor to the end of the line before retrieving the line.
        /// </summary>
        private void OnEnterKeyPressed()
        {
            _textEditor.StopCompletion();
            if (_textEditor.WriteInProgress) return;
            lock (_previousLines)
            {
                // Move cursor to the end of the line.
                _textEditor.Column = GetLastTextEditorLine().Length + 1;

                // Append line.
                string currentLine = GetCurrentLine();
                _previousLines.Add(currentLine);
                _commandLineHistory.Add(currentLine);

                _lineReceivedEvent.Set();
            }
        }

        private string ReadLineFromTextEditor()
        {
            int result = WaitHandle.WaitAny(_waitHandles);
            if (result == _lineReceivedEventIndex)
            {
                lock (_previousLines)
                {
                    string line = _previousLines[0];
                    _previousLines.RemoveAt(0);
                    if (_previousLines.Count == 0)
                    {
                        _lineReceivedEvent.Reset();
                    }
                    return line;
                }
            }
            return null;
        }

        /// <summary>
        /// Replaces the current line text after the prompt with the specified text.
        /// </summary>
        private void ReplaceCurrentLineTextAfterPrompt(string text)
        {
            string currentLine = GetCurrentLine();
            _textEditor.Replace(_promptLength, currentLine.Length, text);

            // Put cursor at end.
            _textEditor.Column = _promptLength + text.Length + 1;
        }

        /// <summary>
        /// Processes characters entered into the text editor by the user.
        /// </summary>
        private void textEditor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                    if (!CanDelete) e.Handled = true;
                    return;

                case Key.Tab:
                    if (IsInReadOnlyRegion) e.Handled = true;
                    return;

                case Key.Back:
                    if (!CanBackspace) e.Handled = true;
                    return;

                case Key.Home:
                    MoveToHomePosition();
                    e.Handled = true;
                    return;

                case Key.Down:
                    if (!IsInReadOnlyRegion) MoveToNextCommandLine();
                    e.Handled = true;
                    return;

                case Key.Up:
                    if (!IsInReadOnlyRegion) MoveToPreviousCommandLine();
                    e.Handled = true;
                    return;
            }
        }

        /// <summary>
        /// Processes characters entering into the text editor by the user.
        /// </summary>
        private void TextEditor_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    _textEditor.RequestCompletioninsertion(e);
                }
            }

            if (IsInReadOnlyRegion)
            {
                e.Handled = true;
            }
            else
            {
                if (e.Text[0] == '\n')
                {
                    OnEnterKeyPressed();
                }

                if (e.Text[0] == '.' && _allowFullAutocompletion)
                {
                    _textEditor.ShowCompletionWindow();
                }

                if ((e.Text[0] == ' ') && (Keyboard.Modifiers == ModifierKeys.Control))
                {
                    e.Handled = true;
                    if (_allowCtrlSpaceAutocompletion) _textEditor.ShowCompletionWindow();
                }
            }
        }

        private void DispatchCommand(Delegate command)
        {
            if (command != null)
            {
                // Slightly involved form to enable keyboard interrupt to work.
                Executing = true;
                var operation = dispatcher.BeginInvoke(DispatcherPriority.Normal, command);
                while (Executing)
                {
                    if (operation.Status != DispatcherOperationStatus.Completed)
                        operation.Wait(TimeSpan.FromSeconds(1));
                    if (operation.Status == DispatcherOperationStatus.Completed)
                        Executing = false;
                }
            }
        }

        public event ConsoleInitializedEventHandler ConsoleInitialized;

        public event EventHandler<EventArgs> ScriptFinished;

        public event EventHandler<EventArgs> ScriptStarting;

        public PythonConsole()
        {
            _executing = false;
            _allowCtrlSpaceAutocompletion = false;
            _allowFullAutocompletion = true;
            _consoleInitialized = false;
            _disableAutocompletionForCallables = true;
            _promptLength = 4;
            _commandLineHistory = new CommandLineHistory();
            _disposedEvent = new ManualResetEvent(false);
            _lineReceivedEvent = new ManualResetEvent(false);
            _lineReceivedEventIndex = 0;
            _previousLines = new List<string>();
            // The index into the waitHandles array where the lineReceivedEvent is stored.
            _statementsExecutionRequestedEvent = new AutoResetEvent(false);
        }

        public PythonConsole(PythonTextEditor textEditor, CommandLine commandLine): this()
        {
            _waitHandles = new WaitHandle[] { _lineReceivedEvent, _disposedEvent };

            _commandLine = commandLine;
            _textEditor = textEditor;
            textEditor.CompletionProvider = new PythonConsoleCompletionDataProvider(commandLine) { ExcludeCallables = _disableAutocompletionForCallables };
            textEditor.PreviewKeyDown += textEditor_PreviewKeyDown;
            textEditor.TextEntering += TextEditor_TextEntering;
            _dispatcherThread = new Thread(new ThreadStart(DispatcherThreadStartingPoint));
            _dispatcherThread.SetApartmentState(ApartmentState.STA);
            _dispatcherThread.IsBackground = true;
            _dispatcherThread.Start();

            // Only required when running outside REP loop.
            _prompt = ">>> ";

            // Set commands:
            _textEditor.TextArea.Dispatcher.Invoke(new Action(delegate ()
            {
                CommandBinding pasteBinding = null;
                CommandBinding copyBinding = null;
                CommandBinding cutBinding = null;
                CommandBinding undoBinding = null;
                CommandBinding deleteBinding = null;
                foreach (CommandBinding commandBinding in (this._textEditor.TextArea.CommandBindings))
                {
                    if (commandBinding.Command == ApplicationCommands.Paste) pasteBinding = commandBinding;
                    if (commandBinding.Command == ApplicationCommands.Copy) copyBinding = commandBinding;
                    if (commandBinding.Command == ApplicationCommands.Cut) cutBinding = commandBinding;
                    if (commandBinding.Command == ApplicationCommands.Undo) undoBinding = commandBinding;
                    if (commandBinding.Command == ApplicationCommands.Delete) deleteBinding = commandBinding;
                }
                // Remove current bindings completely from control. These are static so modifying them will cause other
                // controls' behaviour to change too.
                if (pasteBinding != null)
                    _textEditor.TextArea.CommandBindings.Remove(pasteBinding);
                if (copyBinding != null)
                    _textEditor.TextArea.CommandBindings.Remove(copyBinding);
                if (cutBinding != null)
                    _textEditor.TextArea.CommandBindings.Remove(cutBinding);
                if (undoBinding != null)
                    _textEditor.TextArea.CommandBindings.Remove(undoBinding);
                if (deleteBinding != null)
                    _textEditor.TextArea.CommandBindings.Remove(deleteBinding);

                _textEditor.TextArea.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, OnPaste, CanPaste));
                _textEditor.TextArea.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, OnCopy, PythonEditingCommandHandler.CanCutOrCopy));
                _textEditor.TextArea.CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, PythonEditingCommandHandler.OnCut, CanCut));
                _textEditor.TextArea.CommandBindings.Add(new CommandBinding(ApplicationCommands.Undo, OnUndo, CanUndo));
                _textEditor.TextArea.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, PythonEditingCommandHandler.OnDelete(ApplicationCommands.NotACommand), CanDeleteCommand));
            }));
            CodeContext codeContext = DefaultContext.Default;
            // Set dispatcher to run on a UI thread independent of both the Control UI thread and thread running the REPL.
            ClrModule.SetCommandDispatcher(codeContext, DispatchCommand);
        }

        public bool AllowCtrlSpaceAutocompletion
        {
            get { return _allowCtrlSpaceAutocompletion; }
            set { _allowCtrlSpaceAutocompletion = value; }
        }

        public bool AllowFullAutocompletion
        {
            get { return _allowFullAutocompletion; }
            set { _allowFullAutocompletion = value; }
        }

        public bool DisableAutocompletionForCallables
        {
            get { return _disableAutocompletionForCallables; }
            set
            {
                if (_textEditor.CompletionProvider != null) _textEditor.CompletionProvider.ExcludeCallables = value;
                _disableAutocompletionForCallables = value;
            }
        }
        public TextWriter ErrorOutput
        {
            get { return null; }
            set { }
        }

        public bool Executing
        {
            get { return _executing; }
            set
            {
                if (value && !_executing && ScriptStarting != null)
                    ScriptStarting(this, null);
                if (!value && _executing && ScriptFinished != null)
                    ScriptFinished(this, null);
                _executing = value;
            }
        }

        /// <summary>
        /// Indicates whether there is a line already read by the console and waiting to be processed.
        /// </summary>
        public bool IsLineAvailable
        {
            get
            {
                lock (_previousLines)
                {
                    return _previousLines.Count > 0;
                }
            }
        }

        public TextWriter Output
        {
            get { return null; }
            set { }
        }

        public ScriptScope ScriptScope
        {
            get { return _commandLine.ScriptScope; }
        }

        public void Dispose()
        {
            _disposedEvent.Set();
            _textEditor.PreviewKeyDown -= textEditor_PreviewKeyDown;
            _textEditor.TextEntering -= TextEditor_TextEntering;
        }

        /// <summary>
        /// Gets the text that is yet to be processed from the console. This is the text that is being
        /// typed in by the user who has not yet pressed the enter key.
        /// </summary>
        public string GetCurrentLine()
        {
            string fullLine = GetLastTextEditorLine();
            return fullLine.Substring(_promptLength);
        }

        /// <summary>
        /// Gets the lines that have not been returned by the ReadLine method. This does not
        /// include the current line.
        /// </summary>
        public string[] GetUnreadLines()
        {
            return _previousLines.ToArray();
        }

        /// <summary>
        /// Returns the next line typed in by the console user. If no line is available this method
        /// will block.
        /// </summary>
        public string ReadLine(int autoIndentSize)
        {
            string indent = String.Empty;
            if (autoIndentSize > 0)
            {
                indent = String.Empty.PadLeft(autoIndentSize);
                Write(indent, Style.Prompt);
            }

            string line = ReadLineFromTextEditor();
            if (line != null)
            {
                return indent + line;
            }
            return null;
        }

        /// <summary>
        /// Run externally provided statements in the Console Engine.
        /// </summary>
        /// <param name="statements"></param>
        public void RunStatements(string statements)
        {
            MoveToHomePosition();

            dispatcher.BeginInvoke(new Action(delegate () { ExecuteStatements(statements); }));
        }

        /// <summary>
        /// Run externally provided statements in the Console Engine.
        /// </summary>
        /// <param name="statements"></param>
        public void RunStatementsSync(string statements)
        {
            MoveToHomePosition();

            using (var evt = new AutoResetEvent(false))
            {
                dispatcher.BeginInvoke(new Action(delegate ()
                {
                    try
                    {
                        ExecuteStatements(statements);
                    }
                    finally
                    {
                        evt.Set();
                    }
                }));

                while (true)
                {
                    if (evt.WaitOne(1))
                        break;

                    System.Windows.Forms.Application.DoEvents();
                }
            }
        }

        public void SetDispatcher(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }
        #region CommandHandling

        private const string LineSelectedType = "MSDEVLineSelect";

        private void CanCut(object target, CanExecuteRoutedEventArgs args)
        {
            if (!CanDelete)
            {
                args.CanExecute = false;
            }
            else
                PythonEditingCommandHandler.CanCutOrCopy(target, args);
        }

        private void CanDeleteCommand(object target, CanExecuteRoutedEventArgs args)
        {
            if (!CanDelete)
            {
                args.CanExecute = false;
            }
            else
                PythonEditingCommandHandler.CanDelete(target, args);
        }

        private void CanPaste(object target, CanExecuteRoutedEventArgs args)
        {
            if (IsInReadOnlyRegion)
            {
                args.CanExecute = false;
            }
            else
                args.CanExecute = true;
        }
        private void CanUndo(object target, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = false;
        }

        private void OnCopy(object target, ExecutedRoutedEventArgs args)
        {
            if (target != _textEditor.TextArea) return;
            if (_textEditor.SelectionLength == 0 && Executing)
            {
                // Send the 'Ctrl-C' abort
                //if (!IsInReadOnlyRegion)
                //{
                MoveToHomePosition();
                //textEditor.Column = GetLastTextEditorLine().Length + 1;
                //textEditor.Write(Environment.NewLine);
                //}
                AbortRunningScript();
                args.Handled = true;
            }
            else PythonEditingCommandHandler.OnCopy(target, args);
        }

        private void OnPaste(object target, ExecutedRoutedEventArgs args)
        {
            if (target != _textEditor.TextArea) return;
            TextArea textArea = _textEditor.TextArea;
            if (textArea != null && textArea.Document != null)
            {
                Debug.WriteLine(Clipboard.GetText(TextDataFormat.Html));

                // convert text back to correct newlines for this document
                string newLine = TextUtilities.GetNewLineFromDocument(textArea.Document, textArea.Caret.Line);
                string text = TextUtilities.NormalizeNewLines(Clipboard.GetText(), newLine);
                string[] commands = text.Split(new String[] { newLine }, StringSplitOptions.None);
                string scriptText = "";
                if (commands.Length > 1)
                {
                    text = newLine;
                    foreach (string command in commands)
                    {
                        text += "... " + command + newLine;
                        scriptText += command.Replace("\t", "   ") + newLine;
                    }
                }

                if (!string.IsNullOrEmpty(text))
                {
                    bool fullLine = textArea.Options.CutCopyWholeLine && Clipboard.ContainsData(LineSelectedType);
                    bool rectangular = Clipboard.ContainsData(RectangleSelection.RectangularSelectionDataType);
                    if (fullLine)
                    {
                        DocumentLine currentLine = textArea.Document.GetLineByNumber(textArea.Caret.Line);
                        if (textArea.ReadOnlySectionProvider.CanInsert(currentLine.Offset))
                        {
                            textArea.Document.Insert(currentLine.Offset, text);
                        }
                    }
                    else if (rectangular && textArea.Selection.IsEmpty)
                    {
                        if (!RectangleSelection.PerformRectangularPaste(textArea, textArea.Caret.Position, text, false))
                            _textEditor.Write(text, false);
                    }
                    else
                    {
                        _textEditor.Write(text, false);
                    }
                }
                textArea.Caret.BringCaretToView();
                args.Handled = true;

                if (commands.Length > 1)
                {
                    _dispatcherWindow.Dispatcher.BeginInvoke(new Action(delegate () { ExecuteStatements(scriptText); }));
                }
            }
        }

        private void OnUndo(object target, ExecutedRoutedEventArgs args)
        {
        }

        public void AbortRunningScript()
        {
            if (Executing)
                _dispatcherThread.Abort(new KeyboardInterruptException(""));
        }

          // This is the type VS 2003 and 2005 use for flagging a whole line copy
        #endregion CommandHandling
        /// <summary>
        /// Writes text to the console.
        /// </summary>
        public void Write(string text, Style style)
        {
            _textEditor.Write(text);
            if (style == Style.Prompt)
            {
                _promptLength = text.Length;
                if (!_consoleInitialized)
                {
                    _consoleInitialized = true;
                    if (ConsoleInitialized != null) ConsoleInitialized(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Writes text followed by a newline to the console.
        /// </summary>
        public void WriteLine(string text, Style style)
        {
            Write(text + Environment.NewLine, style);
        }

        /// <summary>
        /// Writes an empty line to the console.
        /// </summary>
        public void WriteLine()
        {
            Write(Environment.NewLine, Style.Out);
        }
    }
}
