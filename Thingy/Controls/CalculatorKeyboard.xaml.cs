using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Thingy.Controls
{
    /// <summary>
    /// Interaction logic for CalculatorKeyboard.xaml
    /// </summary>
    public partial class CalculatorKeyboard : UserControl
    {
        public CalculatorKeyboard()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ClickCommandProperty =
            DependencyProperty.Register("ClickCommand", typeof(ICommand), typeof(CalculatorKeyboard), new FrameworkPropertyMetadata(null));

        public ICommand ClickCommand
        {
            get { return (ICommand)GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }

        public static readonly DependencyProperty ExecuteCommandProperty =
            DependencyProperty.Register("ExecuteCommand", typeof(ICommand), typeof(CalculatorKeyboard), new FrameworkPropertyMetadata(null));

        public ICommand ExecuteCommand
        {
            get { return (ICommand)GetValue(ExecuteCommandProperty); }
            set { SetValue(ExecuteCommandProperty, value); }
        }
    }
}
