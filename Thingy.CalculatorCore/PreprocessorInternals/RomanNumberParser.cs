using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Thingy.CalculatorCore.PreprocessorInternals
{
    internal class RomanNumberParser : IProcessor
    {
        private Dictionary<string, int> RomanNumbers;

        public RomanNumberParser()
        {
            RomanNumbers = new Dictionary<string, int>
            {
                { "M", 1000}, {"CM", 900}, {"D", 500},
                { "CD", 400}, { "C", 100}, {"XC", 90},
                { "L", 50}, {"XL", 40}, {"X", 10},
                { "IX", 9}, {"V", 5}, { "IV", 4},
                { "I", 1}
            };
        }

        public string PatternMatchRegex
        {
            get { return @"^(I|C|L|M|V|X){1,36}\:(ROMAN)"; }
        }

        public bool Process(string input, out string output)
        {
            try
            {
                if (!Regex.IsMatch(input, PatternMatchRegex))
                    throw new Exception("Pattern match error");

                string[] tokens = input.Split(':');
                output = ParseRoman(tokens[0]).ToString();
                return true;
            }
            catch (Exception)
            {
                output = input;
                return false;
            }
        }

        private long ParseRoman(string input)
        {
            long result = 0;
            string textform = input.ToUpper();

            foreach (var pair in RomanNumbers)
            {
                while (textform.IndexOf(pair.Key) == 0)
                {
                    result += pair.Value;
                    textform = textform.Substring(pair.Key.Length);
                }
            }

            return result;
        }
    }
}
