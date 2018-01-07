using AppLib.MVVM;
using System;
using System.Collections.ObjectModel;
using Thingy.CalculatorCore;
using System.Linq;

namespace Thingy.ViewModels.Calculator
{
    public sealed class CalculatorViewModel : ViewModel<Views.ICalculatorView>, IDisposable
    {
        private string _formula;
        private string _result;
        private CalculatorEngine _engine;
        private const int MaxHistoryCount = 20;
        private IApplication _app;

        public DelegateCommand<string> InsertFormulaCommand { get; private set; }
        public DelegateCommand<string> InsertFunctionFormulaCommand { get; private set; }
        public DelegateCommand<string> InsertHistoryCommand { get; private set; }
        public DelegateCommand ExecuteCommand { get; private set; }
        public DelegateCommand ClearCommand { get; private set; }
        public DelegateCommand ClearHistoryCommand { get; private set; }
        public DelegateCommand BackSpaceCommand { get; private set; }
        public DelegateCommand<string> NumSysInputCommand { get; private set; }

        public ObservableCollection<string> History { get; private set; }
        public ObservableCollection<string> Functions { get; private set; }

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

        public CalculatorEngine Engine
        {
            get { return _engine; }
            set { SetValue(ref _engine, value);  }
        }

        public CalculatorViewModel(Views.ICalculatorView view, IApplication app): base(view)
        {
            _app = app;
            Engine = new CalculatorEngine();
            History = new ObservableCollection<string>();
            ExecuteCommand = Command.ToCommand(Execute);
            InsertFunctionFormulaCommand = Command.ToCommand<string>(InsertFunctionFormula);
            InsertFormulaCommand = Command.ToCommand<string>(InsertFormula);
            ClearCommand = Command.ToCommand(Clear);
            ClearHistoryCommand = Command.ToCommand(ClearHistory);
            BackSpaceCommand = Command.ToCommand(BackSpace);
            InsertHistoryCommand = Command.ToCommand<string>(InsertHistory);
            NumSysInputCommand = Command.ToCommand<string>(NumSysInput);
            Functions = new ObservableCollection<string>(_engine.Functions.OrderBy(x => x));
        }

        private async void NumSysInput(string obj)
        {
            var dialog = new Views.CalculatorDialogs.NumberSystemInput();
            dialog.Init(Convert.ToInt32(obj));
            bool result = await _app.ShowDialog(dialog, "Input number in specified system", null);
            if (result)
            {
                switch (dialog.SelectedNumberSystem)
                {
                    case 2:
                        Formula += $"{dialog.NumberText}:BIN";
                        break;
                    case 8:
                        Formula += $"{dialog.NumberText}:OCT";
                        break;
                    case 16:
                        Formula += $"{dialog.NumberText}:HEX";
                        break;
                    default:
                        Formula += $"{dialog.NumberText}:S{dialog.SelectedNumberSystem}";
                        break;
                }
                View.SwitchToMainKeyboard();
                View.FocusFormulaInput();
            }
        }

        private void InsertFunctionFormula(string obj)
        {
            View.SwitchToMainKeyboard();
            Formula += $"{obj}(";
            View.FocusFormulaInput();
        }

        private void InsertHistory(string obj)
        {
            Formula = obj;
        }

        private void BackSpace()
        {
            if (Formula.Length - 1 > -1)
            {
                Formula = Formula.Substring(0, Formula.Length - 1);
            }
        }

        private void ClearHistory()
        {
            History.Clear();
        }

        private void Clear()
        {
            Formula = string.Empty;
            Result = "0";
        }

        private void InsertFormula(string obj)
        {
            View.SwitchToMainKeyboard();
            Formula += obj;
            View.FocusFormulaInput();
        }

        private void UpdateHistory(string item)
        {
            if (History.Count > 0 && History[History.Count - 1] != item)
            {
                if (History.Count > MaxHistoryCount)
                {
                    History.RemoveAt(0);
                }
                History.Add(item);
            }
            else
            {
                History.Add(item);
            }
        }

        private async void Execute()
        {
            var result = await _engine.Calculate(Formula.Trim());
            UpdateHistory(Formula);
            Formula = string.Empty;
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
            View.SwitchToMainKeyboard();
            View.FocusFormulaInput();
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
