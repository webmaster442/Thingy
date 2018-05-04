using System;
using System.ComponentModel;
using System.Text;

namespace Thingy.Engineering.LogicMinimizer
{
    public class LogicItem : INotifyPropertyChanged
    {
        private bool? _Checked;
        private int _Index;
        private string _BinaryValue;

        public bool? Checked
        {
            get { return _Checked; }
            set 
            {
                _Checked = value;
                FirePropertyChangedEvent("Checked");
            }
        }

        public string BinaryValue
        {
            get { return _BinaryValue; }
            set
            {
                _BinaryValue = value;
                FirePropertyChangedEvent("BinaryValue");
            }
        }

        public int Index
        {
            get { return _Index; }
            set
            {
                _Index = value;
                FirePropertyChangedEvent("Index");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void FirePropertyChangedEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static string GetBinaryValue(int number, int chars)
        {
            string bin = Convert.ToString(number, 2);
            var sb = new StringBuilder();
            for (int i = 0; i < chars - bin.Length; i++)
            {
                sb.Append("0");
            }
            sb.Append(bin);
            return sb.ToString();
        }

        public static LogicItem CreateFromMintermIndex(int index, int chars, bool? value)
        {
            var ret = new LogicItem();
            ret.Index = index;
            ret.BinaryValue = GetBinaryValue(index, chars);
            ret.Checked = value;
            return ret;
        }
    }
}
