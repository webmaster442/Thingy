using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thingy.CalculatorCore
{
    /// <summary>
    /// A decent number to text conversion. Handles negative, floating and scientific notation
    /// Iplementation is Based on: https://github.com/robertgreiner/NumberText/blob/master/NumberText/NumberText.cs
    /// </summary>
    public static class NumberToText
    {
        private static readonly Dictionary<double, string> _numbers;
        private static readonly StringBuilder _output;
        private static Dictionary<double, string> _scales;

        private static double Append(double num, double scale)
        {
            if (num > scale - 1)
            {
                var baseScale = ((long)(num / scale));
                AppendLessThanOneThousand(baseScale);
                _output.AppendFormat("{0} ", _scales[scale]);
                num = num - (baseScale * scale);
            }
            return num;
        }

        private static void AppendFloatingPart(string number)
        {
            var parts = number.Split('.');
            _output.Append(" point ");
            foreach (var part in parts[1])
            {
                var key = Convert.ToDouble(new string(part, 1));
                _output.AppendFormat("{0} ", _numbers[key]);
            }
        }

        private static double AppendLessThanOneThousand(double num)
        {
            if (num > 99)
            {
                var hundreds = ((int)(num / 100));
                _output.AppendFormat("{0} hundred ", _numbers[hundreds]);
                num = num - (hundreds * 100);
            }

            if (num > 20)
            {
                var tens = ((int)(num / 10)) * 10;
                _output.AppendFormat("{0} ", _numbers[tens]);
                num = num - tens;
            }

            if (num > 0)
            {
                _output.AppendFormat("{0} ", _numbers[num]);
                num = 0;
            }

            return num;
        }

        static NumberToText()
        {
            _numbers = new Dictionary<double, string>();
            _scales = new Dictionary<double, string>();
            _numbers.Add(0, "zero");
            _numbers.Add(1, "one");
            _numbers.Add(2, "two");
            _numbers.Add(3, "three");
            _numbers.Add(4, "four");
            _numbers.Add(5, "five");
            _numbers.Add(6, "six");
            _numbers.Add(7, "seven");
            _numbers.Add(8, "eight");
            _numbers.Add(9, "nine");
            _numbers.Add(10, "ten");
            _numbers.Add(11, "eleven");
            _numbers.Add(12, "twelve");
            _numbers.Add(13, "thirteen");
            _numbers.Add(14, "fourteen");
            _numbers.Add(15, "fifteen");
            _numbers.Add(16, "sixteen");
            _numbers.Add(17, "seventeen");
            _numbers.Add(18, "eighteen");
            _numbers.Add(19, "nineteen");
            _numbers.Add(20, "twenty");
            _numbers.Add(30, "thirty");
            _numbers.Add(40, "forty");
            _numbers.Add(50, "fifty");
            _numbers.Add(60, "sixty");
            _numbers.Add(70, "seventy");
            _numbers.Add(80, "eighty");
            _numbers.Add(90, "ninety");
            _numbers.Add(100, "hundred");

            _scales.Add(1E3, "thousand");
            _scales.Add(1E6, "million");
            _scales.Add(1E9, "billion");
            _scales.Add(1E12, "trillion");
            _scales.Add(1E15, "quadrillion");
            _scales.Add(1E18, "quintillion");
            _scales.Add(1E21, "sextillion");
            _scales.Add(1E24, "septillion");
            _scales.Add(1E27, "octillion");
            _scales.Add(1E30, "nonillion");
            _scales.Add(1E33, "decillion");
            _scales.Add(1E36, "undecillion");
            _scales.Add(1E39, "duodecillion");
            _scales.Add(1E42, "tredecillion");
            _scales.Add(1E45, "quattuordecillion");
            _scales.Add(1E48, "quindecillion");
            _scales.Add(1E51, "sexdecillion");
            _scales.Add(1E54, "septendecillion");
            _scales.Add(1E57, "octodecillion");
            _scales.Add(1E60, "novemdecillion");
            _scales.Add(1E63, "vigintillion");
            _output = new StringBuilder();
        }

        public static string NumberText(double number)
        {
            try
            {
                _output.Clear();

                if (number == 0) return _numbers[number];
                else if (double.IsNaN(number)) return "Not a number";
                else if (double.IsNegativeInfinity(number)) return "Negative Infinity";
                else if (double.IsPositiveInfinity(number)) return "Positive Infinity";
                else
                {
                    if (number < 0)
                    {
                        _output.Append("minus ");
                        number = Math.Abs(number);
                    }

                    bool floats = (number - Math.Truncate(number)) != 0;
                    string textrep = number.ToString();

                    if (textrep.Contains("E"))
                    {
                        //scientific format
                        var exps = textrep.Split('E');
                        var n = NumberText(Convert.ToDouble(exps[0]));
                        var e = NumberText(Convert.ToDouble(exps[1]));
                        return string.Format("{0} times ten to the power of {1}", n, e);
                    }
                    else
                    {
                        number = _scales.Aggregate(Math.Truncate(number), (current, scale) => Append(current, scale.Key));
                        number = AppendLessThanOneThousand(number);
                        if (floats) AppendFloatingPart(textrep);
                    }
                }

                return _output.ToString();
            }
            catch (Exception)
            {
                return "This number can't be converted to text";
            }
        }
    }
}
