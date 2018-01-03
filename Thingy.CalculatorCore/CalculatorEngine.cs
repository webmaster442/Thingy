using AppLib.Common.IO;
using AppLib.Maths;
using IronPython;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Thingy.CalculatorCore
{
    public sealed class CalculatorEngine : ICalculatorEngine, IDisposable
    {
        private ScriptEngine _engine;
        private ScriptScope _scope;
        private ZeroStream _history;
        private EventRedirectedStreamWriter _output;
        private Dictionary<string, string> _functioncache;
        private FunctionLoader _loader;
        private Preprocessor _preprocessor;

        public CalculatorEngine()
        {
            _functioncache = new Dictionary<string, string>();
            _preprocessor = new Preprocessor();
            var options = new Dictionary<string, object>();
            options["DivisionOptions"] = PythonDivisionOptions.New;
            _history = new ZeroStream();
            _output = new EventRedirectedStreamWriter(_history);
            _output.StreamWasWritten += _output_StreamWasWritten;
            _engine = Python.CreateEngine(options);
            _engine.Runtime.IO.SetOutput(_history, _output);
            _engine.Runtime.IO.SetErrorOutput(_history, _output);
            _scope = _engine.CreateScope();

            _loader = new FunctionLoader(_scope, _functioncache);
            _loader.LoadTypesToScope(typeof(Trigonometry));
        }

        private void _output_StreamWasWritten(object sender, string e)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> Functions
        {
            get { return _functioncache.Keys; }
        }

        public bool PreferPrefixes { get; set; }

        public bool GroupByThousands { get; set; }

        public TrigonometryMode TrigonometryMode
        {
            get { return Trigonometry.Mode; }
            set { Trigonometry.Mode = value; }
        }

        public Task<CalculatorResult> Calculate(string commandLine)
        {
            return Task.Run(() =>
            {
                try
                {
                    if (string.IsNullOrEmpty(commandLine)) return new CalculatorResult(Status.ResultOk, "0");
                    var processed = _preprocessor.Process(commandLine);
                    ScriptSource source = _engine.CreateScriptSourceFromString(processed, SourceCodeKind.AutoDetect);
                    object result = source.Execute(_scope);
                    if (result != null)
                    {
                        _scope.SetVariable("last", result);
                        return new CalculatorResult(Status.ResultOk, StringFormatter.DisplayString(result, PreferPrefixes, GroupByThousands, TrigonometryMode));
                    }
                    else return new CalculatorResult(Status.NoResult, null);
                }
                catch (Exception ex)
                {
                    return new CalculatorResult(Status.ResultError, ex.Message);
                }
            });
        }

        public void Dispose()
        {
            if (_output != null)
            {
                _output.Dispose();
                _output = null;
            }
            if (_history != null)
            {
                _history.Dispose();
                _history = null;
            }
        }
    }
}
