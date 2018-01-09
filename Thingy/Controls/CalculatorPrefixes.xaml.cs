using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Thingy.Controls
{
    /// <summary>
    /// Interaction logic for Prefixes.xaml
    /// </summary>
    public partial class CalculatorPrefixes : UserControl
    {
        public CalculatorPrefixes()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ClickCommandProperty =
            DependencyProperty.Register("CancelCommand", typeof(ICommand), typeof(CalculatorPrefixes), new FrameworkPropertyMetadata(null));

        public ICommand ClickCommand
        {
            get { return (ICommand)GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }
    }
}
