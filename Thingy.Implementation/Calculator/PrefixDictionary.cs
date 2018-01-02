using System.Collections.Generic;
using System.Linq;

namespace Thingy.Implementation.Calculator
{
    /// <summary>
    /// Dictionary override with prefixes
    /// </summary>
    internal class PrefixDictionary : Dictionary<string, double>
    {
        public PrefixDictionary() : base(19)
        {
            Add("y", 1E-24);
            Add("z", 1E-21);
            Add("a", 1E-18);
            Add("f", 1E-15);
            Add("p", 1E-12);
            Add("n", 1E-9);
            Add("u", 1E-6);
            Add("m", 1E-3);
            Add("c", 1E-2);
            Add("d", 1E-1);
            Add("da", 1E1);
            Add("h", 1E2);
            Add("k", 1E3);
            Add("M", 1E6);
            Add("G", 1E9);
            Add("T", 1E12);
            Add("P", 1E15);
            Add("E", 1E18);
            Add("Z", 1E21);
            Add("Y", 1E24);
        }

        /// <summary>
        /// Divides a double value to the nearest corresponding prefix value
        /// </summary>
        /// <param name="value">value to divide</param>
        /// <returns>return string</returns>
        public string DivideToPrefix(double value)
        {
            if (((value < 999) && (value >= 0.001)) || value == 1 || value == 0) return value.ToString();
            double final = value;
            string text = "";
            var sorted = from i in this where i.Value > 999 || i.Value <= 0.001 orderby i.Value descending select i;
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
