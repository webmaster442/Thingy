using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Thingy.API;

namespace Thingy.Mpv.Views
{
    /// <summary>
    /// Interaction logic for MpvView.xaml
    /// </summary>
    public partial class MpvView : UserControl
    {
        private IApplication _app;

        public MpvView(IApplication app)
        {
            InitializeComponent();
            _app = app;
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "Files|*.*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TbProgramName.Text = $"\"{dialog.FileName}\"";
            }
        }
    }
}
