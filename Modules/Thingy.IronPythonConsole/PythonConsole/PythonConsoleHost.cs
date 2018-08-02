// Copyright (c) 2010 Joe Moorhouse

using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Hosting.Providers;
using Microsoft.Scripting.Hosting.Shell;
using System;
using System.Linq;
using System.Text;
using System.Threading;

namespace PythonConsoleControl
{
    public delegate void ConsoleCreatedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Hosts the python console.
    /// </summary>
    public sealed class PythonConsoleHost : ConsoleHost, IDisposable
    {
        private PythonConsole _pythonConsole;
        private PythonTextEditor _textEditor;
        private Thread _thread;

        /// <summary>
        /// Runs the console.
        /// </summary>
        private void RunConsole()
        {
            Run(new string[] { "-X:FullFrames" });
        }

        protected override Type Provider
        {
            get { return typeof(PythonContext); }
        }

        protected override CommandLine CreateCommandLine()
        {
            return new PythonCommandLine();
        }

        /// <remarks>
        /// After the engine is created the standard output is replaced with our custom Stream class so we
        /// can redirect the stdout to the text editor window.
        /// This can be done in this method since the Runtime object will have been created before this method
        /// is called.
        /// </remarks>
        protected override IConsole CreateConsole(ScriptEngine engine, CommandLine commandLine, ConsoleOptions options)
        {
            SetOutput(new PythonOutputStream(_textEditor));
            _pythonConsole = new PythonConsole(_textEditor, commandLine);
            ConsoleCreated?.Invoke(this, EventArgs.Empty);
            return _pythonConsole;
        }

        protected override OptionsParser CreateOptionsParser()
        {
            return new PythonOptionsParser();
        }

        protected override ScriptRuntimeSetup CreateRuntimeSetup()
        {
            ScriptRuntimeSetup srs = ScriptRuntimeSetup.ReadConfiguration();
            foreach (var langSetup in srs.LanguageSetups)
            {
                if (langSetup.FileExtensions.Contains(".py"))
                {
                    langSetup.Options["SearchPaths"] = new string[0];
                }
            }
            return srs;
        }

        protected override void ExecuteInternal()
        {
            if (PythonConfig.SearchPaths?.Any() ?? false)
            {
                var paths = Engine.GetSearchPaths().ToList();
                paths.AddRange(PythonConfig.SearchPaths);
                Engine.SetSearchPaths(paths);
            }

            var pc = HostingHelpers.GetLanguageContext(Engine) as PythonContext;
            pc.SetModuleState(typeof(ScriptEngine), Engine);
            base.ExecuteInternal();
        }

        protected override void ParseHostOptions(string[] args)
        {
            // Python doesn't want any of the DLR base options.
            foreach (string s in args)
            {
                Options.IgnoredArgs.Add(s);
            }
        }

        private void SetOutput(PythonOutputStream stream)
        {
            Runtime.IO.SetOutput(stream, Encoding.UTF8);
        }

        public event ConsoleCreatedEventHandler ConsoleCreated;

        public PythonConsoleHost(PythonTextEditor textEditor)
        {
            this._textEditor = textEditor;
        }

        public PythonConsole Console
        {
            get { return _pythonConsole; }
        }

        public void Dispose()
        {
            if (_pythonConsole != null)
            {
                _pythonConsole.Dispose();
                _pythonConsole = null;
            }

            if (_thread != null)
            {
                _thread.Join();
                _thread = null;
            }
        }

        /// <summary>
        /// Runs the console host in its own thread.
        /// </summary>
        public void Run()
        {
            _thread = new Thread(RunConsole)
            {
                IsBackground = true
            };
            _thread.Start();
        }
    }
}
