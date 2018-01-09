using System;
using System.Text.RegularExpressions;

namespace Thingy.CalculatorCore.PreprocessorInternals
{
    public class CustomNumberSystemParser : IProcessor
    {
        public string PatternMatchRegex
        {
            get { return @"^[0-9A-Za-z]{1,16}\:(S[0-9]{1,2})"; }
        }

        public bool Process(string input, out string output)
        {
            try
            {
                if (!Regex.IsMatch(input, PatternMatchRegex))
                    throw new Exception("Pattern match error");

                string[] tokens = input.Split(':');
                var system = Convert.ToInt32(tokens[1].Replace("S", ""));
                output = NumberSystemConversion.FromSystem(tokens[0], system).ToString();
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
