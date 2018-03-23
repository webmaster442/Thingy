using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Thingy.CalculatorCore.Constants;

namespace Thingy.CalculatorCore.PreprocessorInternals
{
    internal class ConstantParser: IProcessor
    {
        private IConstantDB _db;

        public ConstantParser(IConstantDB db)
        {
            _db = db;
        }

        public string PatternMatchRegex
        {
            get { return "^C:[A-Za-z1-9]+"; }
        }

        public bool Process(string input, out string output)
        {
            try
            {
                if (!Regex.IsMatch(input, PatternMatchRegex))
                    throw new Exception("Pattern match error");

                string[] tokens = input.Split(':');

                var str = DoubleConverter.ToExactString(_db.Lookup(tokens[1]).Value);
                output = str;
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
