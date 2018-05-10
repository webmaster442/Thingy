using AppLib.MVVM;
using System;
using System.Net;
using System.Text;
using Thingy.Engineering.Domain.IP;

namespace Thingy.Engineering.ViewModels
{
    public class SubnetCalculatorViewModel: ViewModel
    {
        private IPAddress _ip;
        private IPAddress _mask;
        private int _networks;
        private int _netmasklen;

        public IPAddress Ip
        {
            get { return _ip; }
            set { SetValue(ref _ip, value); }
        }

        public IPAddress Mask
        {
            get { return _mask; }
            set
            {
                SetValue(ref _mask, value);
                if (value != null)
                    _netmasklen = SubnetMask.GetBitLength(_mask);
                else
                    _netmasklen = 0;
                OnPropertyChanged(() => NetmaskLength);
            }
        }

        public int NetmaskLength
        {
            get { return _netmasklen; }
            set
            {
                SetValue(ref _netmasklen, value);
                if (value >= 2)
                    _mask = SubnetMask.CreateByNetBitLength(_netmasklen);
                OnPropertyChanged(() => Mask);
            }
        }

        public int NetworkCount
        {
            get { return _networks; }
            set { SetValue(ref _networks, value); }
        }

        public DelegateCommand SplitSubnetsCommand { get; }
        public DelegateCommand<string> TemplateCommand { get; }

        public SubnetCalculatorViewModel()
        {
            Ip = IPAddress.Parse("192.168.0.1");
            Mask = IPAddress.Parse("255.255.255.0");
            SplitSubnetsCommand = Command.ToCommand(SplitSubnets);
            TemplateCommand = Command.ToCommand<string>(Template);
        }

        private void Template(string obj)
        {
            Mask = IPAddress.Parse(obj);
        }

        public string Output
        {
            get;
            set;
        }

        private int GetBits(int networks)
        {
            if (networks > 128) return 8;
            else if (networks > 64) return 7;
            else if (networks > 32) return 6;
            else if (networks > 16) return 5;
            else if (networks > 8) return 4;
            else if (networks > 4) return 3;
            else if (networks > 2) return 2;
            else return 1;
        }

        private int GetByteIndex(int netmasklength)
        {
            if (netmasklength > 23) return 3;
            else if (netmasklength > 15) return 2;
            else if (netmasklength > 7) return 1;
            else return 0;
        }

        private void SplitSubnets()
        {
            if (Mask == null || Ip == null) return;

            var buffer = new StringBuilder();
            int requiredbits = GetBits(NetworkCount);

            int maskbits = SubnetMask.GetBitLength(Mask);
            int outmaskbits = requiredbits + maskbits;

            if (outmaskbits > 32)
            {
                buffer.AppendLine("IPv4 is too small for this");
                Output = buffer.ToString();
                OnPropertyChanged(() => Output);
                return;
            }

            buffer.AppendFormat("Required additional subnet bits for {0} networks: {1}\r\n", NetworkCount, requiredbits);
            buffer.AppendFormat("New netmask: {0}/{1}\r\n", SubnetMask.CreateByNetBitLength(outmaskbits), outmaskbits);
            buffer.AppendLine("--------------------------------------------------------------------------------------");
            buffer.AppendLine();

            uint tmp = Ip.GetUint();
            int shift = 32 - outmaskbits;
            for (uint i = 0; i < NetworkCount; i++)
            {
                uint sh = i << shift;
                uint val = tmp + sh;
                var addr = new IPAddress(0);
                addr = addr.SetUint(val);

                buffer.AppendFormat("Subnet {0,-4} Network adress: {1,-15} Broadcast Adress: {2,-15}\r\n",
                                    i, 
                                    addr, 
                                    addr.GetBroadcastAddress(SubnetMask.CreateByNetBitLength(outmaskbits)));
            }

            Output = buffer.ToString();
            OnPropertyChanged(() => Output);
        }
    }
}