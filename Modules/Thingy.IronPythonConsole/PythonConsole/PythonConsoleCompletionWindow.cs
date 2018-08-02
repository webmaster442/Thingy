// Copyright (c) 2010 Joe Moorhouse

using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Rendering;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace PythonConsoleControl
{
    public delegate void DescriptionUpdateDelegate(string description);

    /// <summary>
    /// The code completion window.
    /// </summary>
    public class PythonConsoleCompletionWindow : CompletionWindowBase
    {
        private readonly CompletionList _completionList;
        private PythonTextEditor _textEditor;
        private ToolTip _toolTip = new ToolTip();
        private PythonConsoleCompletionDataProvider _completionDataProvider;
        private DispatcherTimer _updateDescription;
        private TimeSpan _updateDescriptionInterval;

        private void AttachEvents()
        {
            TextArea.Caret.PositionChanged += CaretPositionChanged;
            TextArea.MouseWheel += TextArea_MouseWheel;
            TextArea.PreviewTextInput += TextArea_PreviewTextInput;
        }

        private void CaretPositionChanged(object sender, EventArgs e)
        {
            int offset = TextArea.Caret.Offset;
            if (offset == StartOffset)
            {
                if (CloseAutomatically && CloseWhenCaretAtBeginning)
                {
                    Close();
                }
                else
                {
                    _completionList.SelectItem(string.Empty);
                }
                return;
            }
            if (offset < StartOffset || offset > EndOffset)
            {
                if (CloseAutomatically)
                {
                    Close();
                }
            }
            else
            {
                TextDocument document = TextArea.Document;
                if (document != null)
                {
                    _completionList.SelectItem(document.GetText(StartOffset, offset - StartOffset));
                }
            }
        }

        private void CompletionList_InsertionRequested(object sender, EventArgs e)
        {
            Close();
            // The window must close before Complete() is called.
            // If the Complete callback pushes stacked input handlers, we don't want to pop those when the CC window closes.
            var item = _completionList.SelectedItem;
            if (item != null)
                item.Complete(TextArea, new AnchorSegment(TextArea.Document, StartOffset, EndOffset - StartOffset), e);
        }

        private UIElement GetScrollEventTarget()
        {
            if (_completionList == null)
                return this;
            return _completionList.ScrollViewer ?? _completionList.ListBox ?? (UIElement)_completionList;
        }

        private void TextArea_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = RaiseEventPair(GetScrollEventTarget(),
                                       PreviewMouseWheelEvent, MouseWheelEvent,
                                       new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta));
        }

        private void TextArea_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = RaiseEventPair(this, PreviewTextInputEvent, TextInputEvent,
                                       new TextCompositionEventArgs(e.Device, e.TextComposition));
        }

        /// <inheritdoc/>
        protected override bool CloseOnFocusLost
        {
            get { return CloseAutomatically; }
        }

        /// <inheritdoc/>
        protected override void DetachEvents()
        {
            TextArea.Caret.PositionChanged -= CaretPositionChanged;
            TextArea.MouseWheel -= TextArea_MouseWheel;
            TextArea.PreviewTextInput -= TextArea_PreviewTextInput;
            base.DetachEvents();
        }

        /// <inheritdoc/>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (_toolTip != null)
            {
                _toolTip.IsOpen = false;
                _toolTip = null;
            }
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (!e.Handled)
            {
                _completionList.HandleKey(e);
            }
        }

        /// <summary>
        /// Creates a new code completion window.
        /// </summary>
        public PythonConsoleCompletionWindow(TextArea textArea, PythonTextEditor textEditor)
            : base(textArea)
        {
            _completionList = new CompletionList();

            // keep height automatic
            _completionDataProvider = textEditor.CompletionProvider;
            _textEditor = textEditor;
            CloseAutomatically = true;
            SizeToContent = SizeToContent.Height;
            MaxHeight = 300;
            Width = 250;
            Content = _completionList;
            // prevent user from resizing window to 0x0
            MinHeight = 15;
            MinWidth = 30;

            _toolTip.PlacementTarget = this;
            _toolTip.Placement = PlacementMode.Right;
            _toolTip.Closed += ToolTip_Closed;

            _completionList.InsertionRequested += CompletionList_InsertionRequested;
            _completionList.SelectionChanged += CompletionList_SelectionChanged;
            AttachEvents();

            _updateDescription = new DispatcherTimer();
            _updateDescription.Tick += new EventHandler(CompletionListUpdateDescription);
            _updateDescriptionInterval = TimeSpan.FromSeconds(0.3);

            EventInfo eventInfo = typeof(TextView).GetEvent("ScrollOffsetChanged");
            Delegate methodDelegate = Delegate.CreateDelegate(eventInfo.EventHandlerType, (this as CompletionWindowBase), "TextViewScrollOffsetChanged");
            eventInfo.RemoveEventHandler(TextArea.TextView, methodDelegate);
        }

        /// <summary>
        /// Gets/Sets whether the completion window should close automatically.
        /// The default value is true.
        /// </summary>
        public bool CloseAutomatically
        {
            get;
            set;
        }

        /// <summary>
        /// When flag is set, code completion closes if the caret moves to the
        /// beginning of the allowed range. This is useful in Ctrl+Space and "complete when typing",
        /// but not in dot-completion.
        /// Has no effect if CloseAutomatically is false.
        /// </summary>
        public bool CloseWhenCaretAtBeginning { get; set; }

        /// <summary>
        /// Gets the completion list used in completion window.
        /// </summary>
        public CompletionList CompletionList
        {
            get { return _completionList; }
        }
        #region ToolTip handling

        private void CompletionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = _completionList.SelectedItem;
            if (item == null)
            {
                _updateDescription.Stop();
                return;
            }
            _updateDescription.Interval = _updateDescriptionInterval;
            _updateDescription.Start();
        }

        private void CompletionList_WriteDescription(string description)
        {
            _textEditor.TextEditor.Dispatcher.Invoke(new Action(delegate ()
            {
                if (_toolTip != null)
                {
                    if (description != null)
                    {
                        _toolTip.Content = description;
                        _toolTip.IsOpen = true;
                    }
                    else
                    {
                        _toolTip.IsOpen = false;
                    }
                }
            }));
        }

        private void CompletionListUpdateDescription(Object sender, EventArgs e)
        {
            _updateDescription.Stop();
            _textEditor.UpdateCompletionDescription();
        }

        private void ToolTip_Closed(object sender, RoutedEventArgs e)
        {
            // Clear content after tooltip is closed.
            // We cannot clear is immediately when setting IsOpen=false
            // because the tooltip uses an animation for closing.
            if (_toolTip != null)
                _toolTip.Content = null;
        }
        /// <summary>
        /// Update the description of the current item. This is typically called from a separate thread from the main UI thread.
        /// </summary>
        internal void UpdateCurrentItemDescription()
        {
            if (_textEditor.StopCompletion())
            {
                _updateDescription.Interval = _updateDescriptionInterval;
                _updateDescription.Start();
                return;
            }
            string stub = "";
            string item = "";
            bool isInstance = false;
            _textEditor.TextEditor.Dispatcher.Invoke(new Action(delegate ()
            {
                PythonCompletionData data = (_completionList.SelectedItem as PythonCompletionData);
                if (data == null || _toolTip == null)
                    return;
                stub = data.Stub;
                item = data.Text;
                isInstance = data.IsInstance;
            }));
            // Send to the completion thread to generate the description, providing callback.
            _completionDataProvider.GenerateDescription(stub, item, CompletionList_WriteDescription, isInstance);
        }
        #endregion ToolTip handling
    }
}
