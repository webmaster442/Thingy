using System;
using System.Collections.Generic;
using System.Globalization;
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
using Thingy.Controls;

namespace Thingy.Views.CalculatorDialogs
{
    /// <summary>
    /// Interaction logic for BinaryKeyboard.xaml
    /// </summary>
    public partial class BinaryKeyboard : UserControl
    {
        private bool _loaded;

        public BinaryKeyboard()
        {
            InitializeComponent();
        }

        public string Result
        {
            get;
            set;
        }

        private byte GetValueOrDefault(BinaryEditor binaryEditor)
        {
            if (binaryEditor.IsEnabled) return binaryEditor.ByteValue;
            else return 0;
        }

        private void GetResult()
        {
            if (!_loaded) return;

            if (TypeFloat.IsChecked == true)
            {
                byte[] f = new byte[]
                {
                    GetValueOrDefault(Address0),
                    GetValueOrDefault(Address1),
                    GetValueOrDefault(Address2),
                    GetValueOrDefault(Address3)
                };
                Result = BitConverter.ToSingle(f, 0).ToString(new CultureInfo("en-US"));
            }
            else if (TypeDouble.IsChecked == true)
            {
                byte[] d = new byte[]
                {
                    GetValueOrDefault(Address0),
                    GetValueOrDefault(Address1),
                    GetValueOrDefault(Address2),
                    GetValueOrDefault(Address3),
                    GetValueOrDefault(Address4),
                    GetValueOrDefault(Address5),
                    GetValueOrDefault(Address6),
                    GetValueOrDefault(Address7)
                };
                Result = BitConverter.ToDouble(d, 0).ToString(new CultureInfo("en-US"));
            }
            else
            {
                byte[] i = new byte[]
                {
                    GetValueOrDefault(Address0),
                    GetValueOrDefault(Address1),
                    GetValueOrDefault(Address2),
                    GetValueOrDefault(Address3),
                    GetValueOrDefault(Address4),
                    GetValueOrDefault(Address5),
                    GetValueOrDefault(Address6),
                    GetValueOrDefault(Address7)
                };

                if (Signed.IsChecked == true)
                {
                    Result = BitConverter.ToInt64(i, 0).ToString(new CultureInfo("en-US"));
                }
                else
                {
                    Result = BitConverter.ToUInt64(i, 0).ToString(new CultureInfo("en-US"));
                }
            }
            Preview.Text = Result;
        }

        private void BinaryEditor_BinaryValueChanged(object sender, RoutedEventArgs e)
        {
            GetResult();
        }

        private async void Radio_Checked(object sender, RoutedEventArgs e)
        {
            await Task.Delay(100);
            GetResult();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _loaded = true;
        }
    }
}
