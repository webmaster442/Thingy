using System;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

namespace Thingy.Engineering.Controls
{
    /// <summary>
    /// Interaction logic for IpInput.xaml
    /// </summary>
    public partial class IpInput : UserControl
    {
        private bool _internaltrigger;

        public IpInput()
        {
            InitializeComponent();
        }

        public IPAddress IpAddress
        {
            get { return (IPAddress)GetValue(IpAddressProperty); }
            set { SetValue(IpAddressProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IpAddress.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IpAddressProperty =
            DependencyProperty.Register("IpAddress", typeof(IPAddress), typeof(IpInput), new PropertyMetadata(new IPAddress(0), AddressChanged));

        private static void AddressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ip = e.NewValue as IPAddress;
            if (ip == null || ip.AddressFamily != AddressFamily.InterNetwork)
            {
                ip = new IPAddress(0);

                var sender = d as IpInput;

                if (sender == null || sender._internaltrigger) return;

                byte[] bytes = ip.GetAddressBytes();
                sender.Octet0.Value = bytes[0];
                sender.Octet1.Value = bytes[1];
                sender.Octet2.Value = bytes[2];
                sender.Octet3.Value = bytes[3];
            }

        }

        private void Octet_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            _internaltrigger = true;

            if (Octet0 == null ||
                Octet1 == null || 
                Octet2 == null || 
                Octet3 == null) return;

            IpAddress = new IPAddress(new byte[] 
            {
                Convert.ToByte(Octet0.Value),
                Convert.ToByte(Octet1.Value),
                Convert.ToByte(Octet2.Value),
                Convert.ToByte(Octet3.Value)
            });

            _internaltrigger = false;
        }
    }
}
