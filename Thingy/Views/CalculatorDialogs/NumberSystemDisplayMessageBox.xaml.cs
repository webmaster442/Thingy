using MahApps.Metro.Controls.Dialogs;
using Thingy;

namespace Thingy.Views.CalculatorDialogs
{
    /// <summary>
    /// Interaction logic for NumberSystemDisplayDialog.xaml
    /// </summary>
    public partial class NumberSystemDisplayMessageBox : CustomDialog
    {
        private IApplication _app;

        public NumberSystemDisplayMessageBox(IApplication application)
        {
            InitializeComponent();
            _app = application;
        }

        public void SetDisplay(object o)
        {
            NumBox.DisplayNumber(o);
        }

        private async void PART_NegativeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await _app.HideMessageBox(this);
        }
    }
}
