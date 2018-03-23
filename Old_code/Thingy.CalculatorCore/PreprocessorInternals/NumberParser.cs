using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Thingy.CalculatorCore.PreprocessorInternals
{
    internal abstract class NumberParser : IProcessor
    {
        private int _system;

        public NumberParser(int system)
        {
            _system = system;
        }

        public abstract string PatternMatchRegex
        {
            get;
        }

        public bool Process(string input, out string output)
        {
            try
            {
                if (!Regex.IsMatch(input, PatternMatchRegex))
                    throw new Exception("Pattern match error");

                string[] tokens = input.Split(':');
                output = Convert.ToInt64(tokens[0], _system).ToString(new CultureInfo("en-US"));
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
