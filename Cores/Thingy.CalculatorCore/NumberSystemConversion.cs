using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thingy.CalculatorCore
{
    public static class NumberSystemConversion
    {
        private static Dictionary<byte, char> Digits;

        /// <summary>
        /// Reverse a stringbuilder's content
        /// </summary>
        /// <param name="sb">StringBuilder to reverse</param>
        /// <returns>Reversed stringBuilder</returns>
        private static StringBuilder Reverse(StringBuilder sb)
        {
            var ret = new StringBuilder(sb.Length);
            for (int i = sb.Length - 1; i >= 0; i--)
            {
                ret.Append(sb[i]);
            }
            return ret;
        }

        /// <summary>
        /// Converts a number digit to a number
        /// </summary>
        /// <param name="value">number digit</param>
        /// <returns>vallue associated to number digit</returns>
        private static byte ToDigit(char value)
        {
            switch (value)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return Convert.ToByte(value - 48);

                default:
                    var q = from i in Digits where i.Value == value select i.Key;
                    return q.First();
            }
        }

        static NumberSystemConversion()
        {
            Digits = new Dictionary<byte, char>
            {
                {10, 'A'}, {11, 'B'}, {12, 'C'}, {13, 'D'},
                {14, 'E'}, {15, 'F'}, {16, 'G'}, {17, 'H'},
                {18, 'I'}, {19, 'J'}, {20, 'K'}, {21, 'L'},
                {22, 'M'}, {23, 'N'}, {24, 'O'}, {25, 'P'},
                {26, 'Q'}, {27, 'R'}, {28, 'S'}, {29, 'T'},
                {30, 'U'}, {31, 'V'}, {32, 'W'}, {33, 'X'},
                {34, 'Y'}, {35, 'Z'}
            };
        }

        /// <summary>
        /// Convert a byte array to hexadecimal representation
        /// </summary>
        /// <param name="array">array to convert</param>
        /// <returns>hexadecimal string</returns>
        public static string ByteArrayToHex(byte[] array)
        {
            var ret = new StringBuilder();
            foreach (var b in array)
            {
                string s = Convert.ToString(b, 16);
                if (s.Length < 2) s = "0" + s;
                ret.Append(s);
            }
            return ret.ToString();
        }

        public static string FormatBin(string input)
        {
            var buffer = new StringBuilder();
            int counter = 0;
            for (int i = input.Length - 1; i >= 0; i--)
            {
                if (counter == 4)
                {
                    buffer.Append(" ");
                    counter = 0;
                }
                buffer.Append(input[i]);
                counter++;
            }
            for (int i = 0; i < (4 - counter); i++) buffer.Append(" ");
            return Reverse(buffer).ToString();
        }

        /// <summary>
        /// Converts a number back from a system to decimal
        /// </summary>
        /// <param name="input">Input in system</param>
        /// <param name="system">system</param>
        /// <returns>value in decimal</returns>
        public static long FromSystem(string input, int system)
        {
            int exponent = 0;
            long value = 0;
            for (int i = input.Length - 1; i >= 0; i--)
            {
                value += ToDigit(input[i]) * (long)Math.Pow(system, exponent);
                exponent++;
            }
            return value;
        }

        /// <summary>
        /// Convert an integer to a Roman number
        /// </summary>
        /// <param name="number">Number to convert</param>
        /// <returns>A roman number representation of the input</returns>
        public static string IntToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) return "Roman numbers are represented between 1 and 3999";
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + IntToRoman(number - 1000);
            if (number >= 900) return "CM" + IntToRoman(number - 900); //EDIT: i've typed 400 instead 900
            if (number >= 500) return "D" + IntToRoman(number - 500);
            if (number >= 400) return "CD" + IntToRoman(number - 400);
            if (number >= 100) return "C" + IntToRoman(number - 100);
            if (number >= 90) return "XC" + IntToRoman(number - 90);
            if (number >= 50) return "L" + IntToRoman(number - 50);
            if (number >= 40) return "XL" + IntToRoman(number - 40);
            if (number >= 10) return "X" + IntToRoman(number - 10);
            if (number >= 9) return "IX" + IntToRoman(number - 9);
            if (number >= 5) return "V" + IntToRoman(number - 5);
            if (number >= 4) return "IV" + IntToRoman(number - 4);
            if (number >= 1) return "I" + IntToRoman(number - 1);
            throw new ArgumentOutOfRangeException("something bad happened");
        }

        /// <summary>
        /// Converts a number to a target number system
        /// </summary>
        /// <param name="number">number to convert</param>
        /// <param name="system">Target system between 2 and 36</param>
        /// <returns>The number in the specified system</returns>
        public static string ToSystem(long number, int system)
        {
            if (system < 2 || system > 36)
                throw new ArgumentException("System must be between 2 and 36");

            var output = new StringBuilder();
            while (number > 0)
            {
                var digit = number % system;
                if (digit > 9) output.Append(Digits[(byte)digit]);
                else output.Append(digit);
                number /= system;
            }
            return Reverse(output).ToString();
        }
    }
}
