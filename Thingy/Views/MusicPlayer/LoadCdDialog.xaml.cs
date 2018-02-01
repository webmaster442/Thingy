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
using Thingy.MusicPlayerCore;

namespace Thingy.Views.MusicPlayer
{
    /// <summary>
    /// Interaction logic for LoadCdDialog.xaml
    /// </summary>
    public partial class LoadCdDialog : UserControl
    {
        public LoadCdDialog()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CdDrives.ItemsSource = CDInforProvider.AvailableCDDrives;
            if (CdDrives.Items.Count < 0)
            {
                CdDrives.IsEnabled = false;
            }
        }

        public string SelectedDrive
        {
            get
            {
                return CdDrives.SelectedItem as string;
            }
        }
    }
}
