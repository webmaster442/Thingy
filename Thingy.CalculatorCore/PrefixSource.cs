using System.Collections.Generic;
using System.Linq;

namespace Thingy.CalculatorCore
{
    public static class PrefixSource
    {
        public static IDictionary<string, double> PrefixTable { get; set; }

        static PrefixSource()
        {
            PrefixTable = new Dictionary<string, double>(19)
            {
                { "y", 1E-24 },
                { "z", 1E-21 },
                { "a", 1E-18 },
                { "f", 1E-15 },
                { "p", 1E-12 },
                { "n", 1E-9 },
                { "u", 1E-6 },
                { "m", 1E-3 },
                { "c", 1E-2 },
                { "d", 1E-1 },
                { "da", 1E1 },
                { "h", 1E2 },
                { "k", 1E3 },
                { "M", 1E6 },
                { "G", 1E9 },
                { "T", 1E12 },
                { "P", 1E15 },
                { "E", 1E18 },
                { "Z", 1E21 },
                { "Y", 1E24 }
            };
        }

        /// <summary>
        /// Divides a double value to the nearest corresponding prefix value
        /// </summary>
        /// <param name="value">value to divide</param>
        /// <returns>return string</returns>
        public static string DivideToPrefix(double value)
        {
            if (((value < 999) && (value >= 0.001)) || value == 1 || value == 0) return value.ToString();
            double final = value;
            string text = "";
            var sorted = from i in PrefixTable where i.Value > 999 || i.Value <= 0.001 orderby i.Value descending select i;
            foreach (var prefix in sorted)
            {
                if (final >= prefix.Value)
                {
                    final /= prefix.Value;
                    text = prefix.Key;
                    break;
                }
            }
            return string.Format("{0} {1}", final, text);
        }
    }
}
