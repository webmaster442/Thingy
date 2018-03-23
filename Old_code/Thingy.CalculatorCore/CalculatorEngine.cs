using AppLib.Common.IO;
using AppLib.Maths;
using IronPython;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppLib.MVVM;
using Thingy.CalculatorCore.Constants;
using System.Text;
using System.Linq;
using Thingy.CalculatorCore.FunctionCaching;

namespace Thingy.CalculatorCore
{
    public sealed class CalculatorEngine : BindableBase, ICalculatorEngine, IDisposable
    {
        private ScriptEngine _engine;
        private ScriptScope _scope;
        private ZeroStream _history;
        private EventRedirectedStreamWriter _output;
        private Dictionary<string, FunctionInformation> _functioncache;
        private Preprocessor _preprocessor;
        private StringBuilder _linebuffer;

        private IConstantDB _db;
        private bool _PreferPrefixes;
        private bool _GroupByThousands;

        private Type[] _functionTypes;

        public CalculatorEngine()
        {
            TrigonometryMode = TrigonometryMode.DEG;
            ConstantDB = new ConstantDB();
            _linebuffer = new StringBuilder();
            _functioncache = new Dictionary<string, FunctionInformation>();
            _preprocessor = new Preprocessor(_functioncache, ConstantDB);
            var options = new Dictionary<string, object>();
            options["DivisionOptions"] = PythonDivisionOptions.New;
            _history = new ZeroStream();
            _output = new EventRedirectedStreamWriter(_history);
            _output.StreamWasWritten += _output_StreamWasWritten;
            _engine = Python.CreateEngine(options);
            _engine.Runtime.IO.SetOutput(_history, _output);
            _engine.Runtime.IO.SetErrorOutput(_history, _output);
            _scope = _engine.CreateScope();

            _functionTypes = new Type[]
            {
                typeof(Trigonometry),
                typeof(Engineering),
                typeof(GeneralFunctions),
                typeof(Variations),
                typeof(TypeFunctions),
                typeof(Statistics)
            };

            FunctionCache.Fill(ref _functioncache, ref _scope, _functionTypes);
        }

        private void _output_StreamWasWritten(object sender, string e)
        {
            _linebuffer.Append(e);
        }

        public IEnumerable<Tuple<string, string>> FunctionsNamesAndPrototypes
        {
            get
            {
                foreach (var item in _functioncache)
                {
                    yield return Tuple.Create(item.Key,
                                              string.Join("\n", item.Value.Prototypes));
                }
            }
        }

        public bool PreferPrefixes
        {
            get { return _PreferPrefixes; }
            set { SetValue(ref _PreferPrefixes, value); }
        }

        public bool GroupByThousands
        {
            get { return _GroupByThousands; }
            set { SetValue(ref _GroupByThousands, value); }
        }

        public TrigonometryMode TrigonometryMode
        {
            get { return Trigonometry.Mode; }
            set
            {
                Trigonometry.Mode = value;
                OnPropertyChanged(() => TrigonometryMode);
            }
        }

        public IConstantDB ConstantDB
        {
            get { return _db; }
            private set { SetValue(ref _db, value); }
        }

        public Task<CalculatorResult> Calculate(string commandLine)
        {
            return Task.Run(() =>
            {
                try
                {
                    _linebuffer.Clear();

                    if (string.IsNullOrEmpty(commandLine)) return new CalculatorResult(Status.ResultOk, "0", string.Empty, 0.0d);

                    var processed = _preprocessor.Process(commandLine);

                    ScriptSource source = _engine.CreateScriptSourceFromString(processed, SourceCodeKind.AutoDetect);

                    object result = source.Execute(_scope);

                    if (result != null)
                    {
                        _scope.SetVariable("ans", result);
                        return new CalculatorResult(Status.ResultOk,
                                                    StringFormatter.DisplayString(result, PreferPrefixes, GroupByThousands, TrigonometryMode),
                                                    _linebuffer.ToString(),
                                                    result);
                    }
                    else return new CalculatorResult(Status.NoResult, string.Empty, _linebuffer.ToString(), null);
                }
                catch (Exception ex)
                {
                    return new CalculatorResult(Status.ResultError, ex.Message, _linebuffer.ToString(), null);
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

        public IEnumerable<MemoryItem> GetMemory()
        {
            var variables = _scope.GetVariableNames();
            foreach (var variable in variables)
            {
                if (!variable.StartsWith("_") && !_functionTypes.Where(t => t.Name == variable).Any())
                {
                    object var = _scope.GetVariable(variable);

                    var result = new MemoryItem
                    {
                        VariableName = variable,
                        TypeName = var?.GetType()?.Name,
                        Value = StringFormatter.DisplayString(var, false, true, TrigonometryMode)
                    };
                    yield return result;
                }
            }
        }

        public bool DeleteVariableByName(string name)
        {
            return _scope.RemoveVariable(name);
        }

        public void SetVariable(string name, object value)
        {
            _scope.SetVariable(name, value);
        }
    }
}
