// Copyright (c) 2010 Joe Moorhouse

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;

namespace PythonConsoleControl
{
    /// <summary>
    /// Interface console to AvalonEdit and handle autocompletion.
    /// </summary>
    public class PythonTextEditor
    {
        private int _completionEventIndex = 0;
        private AutoResetEvent _completionRequestedEvent = new AutoResetEvent(false);
        private Thread _completionThread;
        private WaitHandle[] _completionWaitHandles;
        private PythonConsoleCompletionWindow _completionWindow = null;
        private int _descriptionEventIndex = 1;
        private AutoResetEvent _descriptionRequestedEvent = new AutoResetEvent(false);
        private Stopwatch _sw;
        private StringBuilder _writeBuffer = new StringBuilder();
        private volatile bool _writeInProgress = false;
        private PythonConsoleCompletionDataProvider completionProvider = null;
        private void CheckAndOutputWriteBuffer(Object stateInfo)
        {
            AutoResetEvent writeCompletedEvent = new AutoResetEvent(false);
            Action action = new Action(delegate ()
            {
                string toWrite;
                lock (_writeBuffer)
                {
                    toWrite = _writeBuffer.ToString();
                    _writeBuffer.Remove(0, _writeBuffer.Length);
                    //writeBuffer.Clear();
                }
                MoveToEnd();
                PerformTextInput(toWrite);
                writeCompletedEvent.Set();
            });
            while (true)
            {
                // Clear writeBuffer and write out.
                TextArea.Dispatcher.BeginInvoke(action, DispatcherPriority.Normal);
                // Check if writeBuffer has refilled in the meantime; if so clear and write out again.
                writeCompletedEvent.WaitOne();
                lock (_writeBuffer)
                {
                    if (_writeBuffer.Length == 0)
                    {
                        _writeInProgress = false;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Perform completion actions on the background completion thread.
        /// </summary>
        private void Completion()
        {
            while (true)
            {
                int action = WaitHandle.WaitAny(_completionWaitHandles);
                if (action == _completionEventIndex && completionProvider != null) BackgroundShowCompletionWindow();
                if (action == _descriptionEventIndex && completionProvider != null && _completionWindow != null) BackgroundUpdateCompletionDescription();
            }
        }

        private void MoveToEnd()
        {
            int lineCount = TextArea.Document.LineCount;

            if (TextArea.Caret.Line != lineCount)
                TextArea.Caret.Line = TextArea.Document.LineCount;

            int column = TextArea.Document.Lines[lineCount - 1].Length + 1;

            if (TextArea.Caret.Column != column)
                TextArea.Caret.Column = column;
        }

        private void PerformTextInput(string text)
        {
            if (text == "\n" || text == "\r\n")
            {
                string newLine = TextUtilities.GetNewLineFromDocument(TextArea.Document, TextArea.Caret.Line);
                TextEditor.AppendText(newLine);
            }
            else
                TextEditor.AppendText(text);
            TextArea.Caret.BringCaretToView();
        }

        internal TextArea TextArea { get; set; }
        internal TextEditor TextEditor { get; set; }

        /// <summary>
        /// Obtain completions (this runs in its own thread)
        /// </summary>
        internal void BackgroundShowCompletionWindow() //ICompletionItemProvider
        {
            // provide AvalonEdit with the data:
            string itemForCompletion = "";
            TextArea.Dispatcher.Invoke(new Action(delegate ()
            {
                DocumentLine line = TextArea.Document.Lines[TextArea.Caret.Line - 1];
                itemForCompletion = TextArea.Document.GetText(line);
            }));

            ICompletionData[] completions = completionProvider.GenerateCompletionData(itemForCompletion);

            if (completions != null && completions.Length > 0) TextArea.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                _completionWindow = new PythonConsoleCompletionWindow(TextArea, this);
                IList<ICompletionData> data = _completionWindow.CompletionList.CompletionData;
                foreach (ICompletionData completion in completions)
                {
                    data.Add(completion);
                }
                _completionWindow.Show();
                _completionWindow.Closed += delegate
                {
                    _completionWindow = null;
                };
            }));
        }

        internal void BackgroundUpdateCompletionDescription()
        {
            _completionWindow.UpdateCurrentItemDescription();
        }

        public delegate string StringAction();

        public event KeyEventHandler PreviewKeyDown
        {
            add { TextArea.PreviewKeyDown += value; }
            remove { TextArea.PreviewKeyDown -= value; }
        }

        public event TextCompositionEventHandler TextEntered
        {
            add { TextArea.TextEntered += value; }
            remove { TextArea.TextEntered -= value; }
        }

        public event TextCompositionEventHandler TextEntering
        {
            add { TextArea.TextEntering += value; }
            remove { TextArea.TextEntering -= value; }
        }

        public PythonTextEditor(TextEditor textEditor)
        {
            TextEditor = textEditor;
            TextArea = textEditor.TextArea;
            _completionWaitHandles = new WaitHandle[] { _completionRequestedEvent, _descriptionRequestedEvent };
            _completionThread = new Thread(new ThreadStart(Completion));
            _completionThread.Priority = ThreadPriority.Lowest;
            //completionThread.SetApartmentState(ApartmentState.STA);
            _completionThread.IsBackground = true;
            _completionThread.Start();
        }

        public int Column
        {
            get { return TextArea.Caret.Column; }
            set { TextArea.Caret.Column = value; }
        }

        public ICollection<CommandBinding> CommandBindings
        {
            get { return (this.TextArea.ActiveInputHandler as TextAreaDefaultInputHandler).CommandBindings; }
        }

        public PythonConsoleCompletionDataProvider CompletionProvider
        {
            get { return completionProvider; }
            set { completionProvider = value; }
        }

        public Thread CompletionThread
        {
            get { return _completionThread; }
        }

        public PythonConsoleCompletionWindow CompletionWindow
        {
            get { return _completionWindow; }
        }

        /// <summary>
        /// Gets the current cursor line.
        /// </summary>
        public int Line
        {
            get { return TextArea.Caret.Line; }
            set { TextArea.Caret.Line = value; }
        }

        public int SelectionEndColumn
        {
            get
            {
                int endOffset = TextArea.Selection.SurroundingSegment.EndOffset;
                return endOffset - TextArea.Document.GetLineByOffset(endOffset).Offset + 1;
            }
        }

        public bool SelectionIsMultiline
        {
            get
            {
                return TextArea.Selection.IsMultiline;
            }
        }

        public int SelectionLength
        {
            get
            {
                return TextArea.Selection.Length;
            }
        }

        public int SelectionStart
        {
            get
            {
                return TextArea.Selection.SurroundingSegment.Offset;
            }
        }

        public int SelectionStartColumn
        {
            get
            {
                int startOffset = TextArea.Selection.SurroundingSegment.Offset;
                return startOffset - TextArea.Document.GetLineByOffset(startOffset).Offset + 1;
            }
        }

        /// <summary>
        /// Gets the total number of lines in the text editor.
        /// </summary>
        public int TotalLines
        {
            get { return TextArea.Document.LineCount; }
        }

        public bool WriteInProgress
        {
            get { return _writeInProgress; }
        }

        /// <summary>
        /// Gets the text for the specified line.
        /// </summary>
        public string GetLine(int index)
        {
            return (string)TextArea.Dispatcher.Invoke(new StringAction(delegate ()
            {
                DocumentLine line = TextArea.Document.Lines[index];
                return TextArea.Document.GetText(line);
            }));
        }

        /// <summary>
        /// Replaces the text at the specified index on the current line with the specified text.
        /// </summary>
        public void Replace(int index, int length, string text)
        {
            //int currentLine = textArea.Caret.Line - 1;
            int currentLine = TextArea.Document.LineCount - 1;
            int startOffset = TextArea.Document.Lines[currentLine].Offset;
            TextArea.Document.Replace(startOffset + index, length, text);
        }

        public void RequestCompletioninsertion(TextCompositionEventArgs e)
        {
            if (_completionWindow != null) _completionWindow.CompletionList.RequestInsertion(e);
            // if autocompletion still in progress, terminate
            StopCompletion();
        }

        public void ShowCompletionWindow()
        {
            _completionRequestedEvent.Set();
        }

        public bool StopCompletion()
        {
            if (completionProvider.AutocompletionInProgress)
            {
                // send Ctrl-C abort
                _completionThread.Abort(new Microsoft.Scripting.KeyboardInterruptException(""));
                return true;
            }
            return false;
        }

        public void UpdateCompletionDescription()
        {
            _descriptionRequestedEvent.Set();
        }

        public void Write(string text)
        {
            Write(text, false);
        }

        public void Write(string text, bool allowSynchronous)
        {
            //text = text.Replace("\r\r\n", "\r\n");
            text = text.Replace("\r\r\n", "\r");
            text = text.Replace("\r\n", "\r");
            if (allowSynchronous)
            {
                MoveToEnd();
                PerformTextInput(text);
                return;
            }
            lock (_writeBuffer)
            {
                _writeBuffer.Append(text);
            }
            if (!_writeInProgress)
            {
                _writeInProgress = true;
                ThreadPool.QueueUserWorkItem(new WaitCallback(CheckAndOutputWriteBuffer));
                _sw = Stopwatch.StartNew();
            }
        }
    }
}
