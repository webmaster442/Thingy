using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using Thingy.API;

namespace Thingy.Calculator.Dialogs
{
    /// <summary>
    /// Interaction logic for MultiLineResultDialog.xaml
    /// </summary>
    public partial class MultiLineResultMessageBox : CustomDialog
    {
        private IApplication _app;

        public MultiLineResultMessageBox(IApplication app)
        {
            InitializeComponent();
            _app = app;
        }

        private async void PART_NegativeButton_Click(object sender, RoutedEventArgs e)
        {
            await _app.HideMessageBox(this);
        }

        public string Text
        {
            get { return TbResult.Text; }
            set { TbResult.Text = value; }
        }
    }
}
