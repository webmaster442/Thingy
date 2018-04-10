using AppLib.MVVM;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Thingy.Calculator.Dialogs
{
    /// <summary>
    /// Interaction logic for NumberSystemInput.xaml
    /// </summary>
    public partial class NumberSystemInput : UserControl
    {
        public int SelectedNumberSystem
        {
            get;
            private set;
        }

        public DelegateCommand<char> InsertCommand { get; private set; }

        public string NumberText
        {
            get { return InputTextBox.Text; }
            set { InputTextBox.Text = value; }
        }

        /// <summary>
        /// Init dialog with given system. If system is bigger than 36 it will be treated as roman system
        /// </summary>
        /// <param name="system">number system</param>
        public void Init(int system)
        {
            SelectedNumberSystem = system;
            if (system > 36)
            {
                SystemSelector.IsEnabled = false;
                Keys.ItemsSource = null;
                Keys.ItemsSource = new char[] { 'I', 'V', 'X', 'L', 'C', 'D', 'M' };
            }
            else
            {
                var source = _supportedSymbols.Take(system).ToList();
                Keys.ItemsSource = null;
                Keys.ItemsSource = source;
                if (system < 36)
                    SystemSelector.IsEnabled = false;
                SystemSelector.Value = system;
            }
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
