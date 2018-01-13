using System.Windows.Controls;
using System.Windows.Input;

namespace Thingy.Views
{
    /// <summary>
    /// Interaction logic for Calculator.xaml
    /// </summary>
    public partial class Calculator : UserControl, ICalculatorView
    {
        public Calculator()
        {
            InitializeComponent();
            FocusFormulaInput();
        }

        public void FocusFormulaInput()
        {
            FocusManager.SetFocusedElement(this, FormulaBox);
            FormulaBox.CaretIndex = FormulaBox.Text.Length;
        }

        public void SwitchToMainKeyboard()
        {
            Dispatcher.Invoke(() =>
            {
                KeyboardViewSwitcher.SelectedIndex = 0;
            });
        }

        private void DisplayModes_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DisplayModes.ContextMenu.IsOpen = true;
        }
    }
}
