using System;
using System.Diagnostics;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Hosting.Providers;
using Microsoft.Scripting.Hosting.Shell;
using IronPython.Hosting;
using IronPython.Runtime;

namespace Thingy.Cmd.Modules
{
    internal sealed class PythonConsoleHost : ConsoleHost
    {

        protected override Type Provider
        {
            get { return typeof(PythonContext); }
        }

        protected override CommandLine CreateCommandLine()
        {
            return new PythonCommandLine();
        }

        protected override OptionsParser CreateOptionsParser()
        {
            return new PythonOptionsParser();
        }

        protected override ScriptRuntimeSetup CreateRuntimeSetup()
        {
            ScriptRuntimeSetup srs = base.CreateRuntimeSetup();
            foreach (var langSetup in srs.LanguageSetups)
            {
                if (langSetup.FileExtensions.Contains(".py"))
                {
                    langSetup.Options["SearchPaths"] = new string[0];
                }
            }
            return srs;
        }

        protected override LanguageSetup CreateLanguageSetup()
        {
            return Python.CreateLanguageSetup(null);
        }

        protected override IConsole CreateConsole(ScriptEngine engine, CommandLine commandLine, ConsoleOptions options)
        {
            PythonConsoleOptions pyoptions = (PythonConsoleOptions)options;
            return pyoptions.BasicConsole ? new BasicConsole(options.ColorfulConsole) : new SuperConsole(commandLine, options.ColorfulConsole);
        }

        protected override void ParseHostOptions(string/*!*/[]/*!*/ args)
        {
            // Python doesn't want any of the DLR base options.
            foreach (string s in args)
            {
                Options.IgnoredArgs.Add(s);
            }
        }

        protected override void ExecuteInternal()
        {
            var pc = HostingHelpers.GetLanguageContext(Engine) as PythonContext;
            pc.SetModuleState(typeof(ScriptEngine), Engine);
            base.ExecuteInternal();
        }

        public static int PythonShell(string[] args)
        {
            // Work around issue w/ pydoc - piping to more doesn't work so
            // instead indicate that we're a dumb terminal
            if (Environment.GetEnvironmentVariable("TERM") == null)
            {
                Environment.SetEnvironmentVariable("TERM", "dumb");
            }

            return new PythonConsoleHost().Run(args);
        }
    }
}