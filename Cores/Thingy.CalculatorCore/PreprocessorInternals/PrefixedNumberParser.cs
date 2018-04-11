using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Thingy.CalculatorCore.PreprocessorInternals
{
    internal class PrefixedNumberParser : IProcessor
    {
        public string PatternMatchRegex
        {
            get { return @"^[0-9.]{1,128}:(y|z|a|f|p|n|u|m|c|d|da|h|k|M|G|T|P|E|Z|Y)"; }
        }

        public bool Process(string input, out string output)
        {
            try
            {
                if (!Regex.IsMatch(input, PatternMatchRegex))
                    throw new Exception("Pattern match error");

                string[] tokens = input.Split(':');
                double value = Convert.ToDouble(tokens[0], new CultureInfo("en-US"));
                double multiplier = PrefixSource.PrefixTable.Where(p => p.Key == tokens[1]).Select(p => p.Value).FirstOrDefault();
                output = (value * multiplier).ToString(new CultureInfo("en-US"));
                return true;
            }
            catch (Exception)
            {
                output = input;
                return false;
            }
        }
    }
}
