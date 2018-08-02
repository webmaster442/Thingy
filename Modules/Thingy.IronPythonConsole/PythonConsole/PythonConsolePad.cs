// Copyright (c) 2010 Joe Moorhouse

using ICSharpCode.AvalonEdit;
using System.Windows.Media;

namespace PythonConsoleControl
{
    public class PythonConsolePad
    {
        private PythonConsoleHost _host;
        private PythonTextEditor _pythonTextEditor;
        private TextEditor _textEditor;

        public PythonConsolePad()
        {
            _textEditor = new TextEditor();
            _pythonTextEditor = new PythonTextEditor(_textEditor);
            _host = new PythonConsoleHost(_pythonTextEditor);
            _host.Run();
            _textEditor.FontFamily = new FontFamily("Consolas");
            _textEditor.FontSize = 12;
        }

        public PythonConsole Console
        {
            get { return _host.Console; }
        }

        public TextEditor Control
        {
            get { return _textEditor; }
        }

        public PythonConsoleHost Host
        {
            get { return _host; }
        }
        public void Dispose()
        {
            _host.Dispose();
        }
    }
}
