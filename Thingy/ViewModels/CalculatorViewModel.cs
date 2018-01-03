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
        private const int MaxHistoryCount = 20;

        public DelegateCommand<string> InsertFormulaCommand { get; private set; }
        public DelegateCommand<string> InsertHistoryCommand { get; private set; }
        public DelegateCommand ExecuteCommand { get; private set; }
        public DelegateCommand ClearCommand { get; private set; }
        public DelegateCommand ClearHistoryCommand { get; private set; }
        public DelegateCommand BackSpaceCommand { get; private set; }

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
            ClearCommand = Command.ToCommand(Clear);
            ClearHistoryCommand = Command.ToCommand(ClearHistory);
            BackSpaceCommand = Command.ToCommand(BackSpace);
            InsertHistoryCommand = Command.ToCommand<string>(InsertHistory);
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
            Formula += obj;
        }

        private void UpdateHistory(string item)
        {
            if (History.Count > MaxHistoryCount)
            {
                History.RemoveAt(0);
            }
            History.Add(item);
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
