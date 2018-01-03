using AppLib.MVVM;
using System;
using System.Collections.ObjectModel;
using Thingy.CalculatorCore;

namespace Thingy.ViewModels
{
    public sealed class CalculatorViewModel : ViewModel, IDisposable
    {
        private string _formula;
        private string _result;
        private CalculatorEngine _engine;

        public DelegateCommand<string> InsertFormulaCommand { get; private set; }
        public DelegateCommand ExecuteCommand { get; private set; }

        public ObservableCollection<string> History { get; private set; }

        public string Formula
        {
            get { return _formula; }
            set { SetValue(ref _formula, value); }
        }

        public string Result
        {
            get { return _result; }
            set { SetValue(ref _result, value); }
        }

        public CalculatorViewModel()
        {
            _engine = new CalculatorEngine();
            History = new ObservableCollection<string>();
            ExecuteCommand = Command.ToCommand(Execute);
            InsertFormulaCommand = Command.ToCommand<string>(InsertFormula);
        }

        private void InsertFormula(string obj)
        {
            Formula += obj;
        }

        private async void Execute()
        {
            var result = await _engine.Calculate(Formula);
            switch (result.Status)
            {
                case Status.ResultOk:
                    Result = result.Content;
                    break;
                case Status.NoResult:
                    Result = "Ok";
                    break;
                case Status.ResultError:
                    Result = $"Error: {result.Content}";
                    break;
            }
        }

        public void Dispose()
        {
            if (_engine != null)
            {
                _engine.Dispose();
                _engine = null;
            }
        }
    }
}
