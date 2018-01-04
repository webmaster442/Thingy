using AppLib.MVVM;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Thingy.Views.CalculatorDialogs
{
    /// <summary>
    /// Interaction logic for NumberSystemInput.xaml
    /// </summary>
    public partial class NumberSystemInput : UserControl
    {

        private int _selectedSystem;

        public int SelectedNumberSystem
        {
            get { return _selectedSystem; }
            set
            {
                _selectedSystem = value;
                var source = _supportedSymbols.Take(value).ToList();
                Keys.ItemsSource = null;
                Keys.ItemsSource = source;
            }
        }

        public DelegateCommand<char> InsertCommand { get; private set; }

        public string NumberText
        {
            get { return InputTextBox.Text; }
            set { InputTextBox.Text = value; }
        }

        public void Init(int system)
        {
            if (system < 36)
                SystemSelector.IsEnabled = false;
            SelectedNumberSystem = system;
            SystemSelector.Value = system;
        }

        private char[] _supportedSymbols;

        public NumberSystemInput()
        {
            InitializeComponent();
            _supportedSymbols = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                                             'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                                             'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'S', 'T', 'U', 'V',
                                             'W', 'X', 'Y','Z'};
            SelectedNumberSystem = 16;
            InsertCommand = Command.ToCommand<char>(Insert);
            FocusManager.SetFocusedElement(this, InputTextBox);
        }

        private void Insert(char obj)
        {
            InputTextBox.Text += obj.ToString();
            InputTextBox.CaretIndex = InputTextBox.Text.Length;
            FocusManager.SetFocusedElement(this, InputTextBox);
        }

        private void SystemSelector_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double?> e)
        {
            SelectedNumberSystem = (int)SystemSelector.Value;
        }
    }
}
