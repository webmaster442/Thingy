using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Thingy.CalculatorCore;

namespace Thingy.Calculator.Controls
{
    /// <summary>
    /// Interaction logic for CalculatorMemoryView.xaml
    /// </summary>
    public partial class CalculatorMemoryView : UserControl
    {
        public CalculatorMemoryView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty MemoryListProperty =
            DependencyProperty.Register("MemoryList", typeof(ObservableCollection<MemoryItem>), typeof(CalculatorMemoryView));

        public ObservableCollection<MemoryItem> MemoryList
        {
            get { return (ObservableCollection<MemoryItem>)GetValue(MemoryListProperty); }
            set { SetValue(MemoryListProperty, value); }
        }

        public static readonly DependencyProperty InsertCommandProperty =
            DependencyProperty.Register("InsertCommand", typeof(ICommand), typeof(CalculatorMemoryView));

        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register("DeleteCommand", typeof(ICommand), typeof(CalculatorMemoryView));

        public static readonly DependencyProperty CancelCommandProperty =
            DependencyProperty.Register("CancelCommand", typeof(ICommand), typeof(CalculatorMemoryView));

        public static readonly DependencyProperty EvalResultCommandProperty =
            DependencyProperty.Register("EvalResultCommand", typeof(ICommand), typeof(CalculatorMemoryView));

        public static readonly DependencyProperty ResultCommandProperty =
            DependencyProperty.Register("ResultCommand", typeof(ICommand), typeof(CalculatorMemoryView));

        public ICommand InsertCommand
        {
            get { return (ICommand)GetValue(InsertCommandProperty); }
            set { SetValue(InsertCommandProperty, value); }
        }

        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        public ICommand CancelCommand
        {
            get { return (ICommand)GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }

        public ICommand EvalResultCommand
        {
            get { return (ICommand)GetValue(EvalResultCommandProperty); }
            set { SetValue(EvalResultCommandProperty, value); }
        }

        public ICommand ResultCommand
        {
            get { return (ICommand)GetValue(ResultCommandProperty); }
            set { SetValue(ResultCommandProperty, value); }
        }
    }
}
