using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Thingy.Controls
{
    /// <summary>
    /// Interaction logic for BinaryEditor.xaml
    /// </summary>
    public partial class BinaryEditor : UserControl
    {
        public static readonly DependencyProperty ByteValueProperty = DependencyProperty.Register(
        "ByteValue", typeof(byte), typeof(BinaryEditor), new PropertyMetadata(default(byte), UpdateUi));

        public static readonly DependencyProperty MsbIndexProperty = DependencyProperty.Register(
            "MsbIndex", typeof(int), typeof(BinaryEditor), new PropertyMetadata(7, MsbIndexChange));

        public static readonly DependencyProperty LsbIndexProperty = DependencyProperty.Register(
            "LsbIndex", typeof(int), typeof(BinaryEditor), new PropertyMetadata(0, LsbIndexChange));

        public BinaryEditor()
        {
            InitializeComponent();
        }

        public byte ByteValue
        {
            get { return (byte)GetValue(ByteValueProperty); }
            set { SetValue(ByteValueProperty, value); }
        }

        public int MsbIndex
        {
            get { return (int)GetValue(MsbIndexProperty); }
            set { SetValue(MsbIndexProperty, value); }
        }

        public int LsbIndex
        {
            get { return (int)GetValue(LsbIndexProperty); }
            set { SetValue(LsbIndexProperty, value); }
        }

        private static void UpdateUi(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var binEditor = d as BinaryEditor;
            if (binEditor == null) return;
            var value = (byte)e.NewValue;

            if (value - 128 >= 0)
            {
                value -= 128;
                binEditor.Val128.IsChecked = true;
            }
            else
            {
                binEditor.Val128.IsChecked = false;
            }

            if (value - 64 >= 0)
            {
                value -= 64;
                binEditor.Val64.IsChecked = true;
            }
            else
            {
                binEditor.Val64.IsChecked = false;
            }

            if (value - 32 >= 0)
            {
                value -= 32;
                binEditor.Val32.IsChecked = true;
            }
            else
            {
                binEditor.Val32.IsChecked = false;
            }

            if (value - 16 >= 0)
            {
                value -= 16;
                binEditor.Val16.IsChecked = true;
            }
            else
            {
                binEditor.Val16.IsChecked = false;
            }

            if (value - 8 >= 0)
            {
                value -= 8;
                binEditor.Val8.IsChecked = true;
            }
            else
            {
                binEditor.Val8.IsChecked = false;
            }

            if (value - 4 >= 0)
            {
                value -= 4;
                binEditor.Val4.IsChecked = true;
            }
            else
            {
                binEditor.Val4.IsChecked = false;
            }

            if (value - 2 >= 0)
            {
                value -= 2;
                binEditor.Val2.IsChecked = true;
            }
            else
            {
                binEditor.Val2.IsChecked = false;
            }

            if (value - 1 >= 0)
            {
                value -= 1;
                binEditor.Val1.IsChecked = true;
            }
            else
            {
                binEditor.Val1.IsChecked = false;
            }
        }

        private static void MsbIndexChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var binEditor = d as BinaryEditor;
            if (binEditor == null) return;
            var str = "2" + CreateUnicodeSuperScript((int)e.NewValue);
            binEditor.MsbLabel.Text = str;
        }

        private static void LsbIndexChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var binEditor = d as BinaryEditor;
            if (binEditor == null) return;
            var str = "2" + CreateUnicodeSuperScript((int)e.NewValue);
            binEditor.LsbLabel.Text = str;
        }

        private static string CreateUnicodeSuperScript(int value)
        {
            var str = Convert.ToString(value);
            var result = new StringBuilder();

            foreach (var chr in str)
                switch (chr)
                {
                    case '0':
                        result.Append('⁰');
                        break;
                    case '1':
                        result.Append('¹');
                        break;
                    case '2':
                        result.Append('²');
                        break;
                    case '3':
                        result.Append('³');
                        break;
                    case '4':
                        result.Append('⁴');
                        break;
                    case '5':
                        result.Append('⁵');
                        break;
                    case '6':
                        result.Append('⁶');
                        break;
                    case '7':
                        result.Append('⁷');
                        break;
                    case '8':
                        result.Append('⁸');
                        break;
                    case '9':
                        result.Append('⁹');
                        break;
                }

            return result.ToString();
        }

        private void UpdateProperty(object sender, RoutedEventArgs e)
        {
            var eventSource = sender as ToggleButton;
            if (eventSource == null) return;

            var current = ByteValue;
            var value = Convert.ToByte(eventSource.Tag);

            if (eventSource.IsChecked == true) current += value;
            else current -= value;

            ByteValue = current;
        }
    }
}