using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls;
using Thingy.CalculatorCore;

namespace Thingy.Controls
{
    internal class NumberSystemTextBox: TextBox
    {
        public NumberSystemTextBox()
        {
            AcceptsReturn = true;
            AcceptsTab = true;
        }

        public void DisplayNumber(object o)
        {
            try
            {
                double doubleValue = Convert.ToDouble(o);
                float floatValue = Convert.ToSingle(o);
                bool isFloatingPoint = (doubleValue - Math.Truncate(doubleValue)) != 0;
                var buffer = new StringBuilder();
                if (isFloatingPoint)
                {
                    byte[] singlebytes = BitConverter.GetBytes(floatValue);
                    Array.Reverse(singlebytes);

                    byte[] doublebytes = BitConverter.GetBytes(doubleValue);
                    Array.Reverse(doublebytes);

                    buffer.AppendFormat("IEEE 754 Double: {0}\r\n", NumberSystemConversion.ByteArrayToHex(doublebytes));
                    buffer.AppendFormat("IEEE 754 Single: {0}", NumberSystemConversion.ByteArrayToHex(singlebytes));
                    Text = buffer.ToString();
                    return;
                }
                else
                {
                    string bin, oct, hex;
                    var integerNumber = Convert.ToInt64(doubleValue);
                    bin = NumberSystemConversion.ToSystem(integerNumber, 2);
                    oct = NumberSystemConversion.ToSystem(integerNumber, 8);
                    hex = NumberSystemConversion.ToSystem(integerNumber, 16);

                    int bits = bin.Length;
                    bin = NumberSystemConversion.FormatBin(bin);

                    buffer.AppendFormat("DEC: {0}\n", integerNumber);
                    buffer.AppendFormat("BIN: {0}\n", bin);
                    buffer.AppendFormat("OCT: {0}\n", oct);
                    buffer.AppendFormat("HEX: {0}\n", hex);
                    buffer.AppendFormat("-------------------------------------\n");
                    buffer.AppendFormat("Bits: {0}", bits);
                }
                Text = buffer.ToString();
            }
            catch (Exception ex)
            {
                Text = "Error displaying number in various systems";
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
