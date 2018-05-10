using AppLib.MVVM;
using System;
using System.Text;

namespace Thingy.Engineering.Domain.LogicMinimizer
{
    public class LogicItem : BindableBase
    {
        private bool? _Checked;
        private int _Index;
        private string _BinaryValue;

        public bool? Checked
        {
            get { return _Checked; }
            set { SetValue(ref _Checked, value); }
        }

        public string BinaryValue
        {
            get { return _BinaryValue; }
            set { SetValue(ref _BinaryValue, value); }
        }

        public int Index
        {
            get { return _Index; }
            set { SetValue(ref _Index, value); }
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
