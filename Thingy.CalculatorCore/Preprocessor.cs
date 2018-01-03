using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Thingy.CalculatorCore.PreprocessorInternals;

namespace Thingy.CalculatorCore
{
    public class Preprocessor
    {
        private readonly string[] _operators;
        private readonly List<IProcessor> _processors;
        private readonly string _tokenizerregex;

        public Preprocessor()
        {
            _operators = new string[] { @"\+", @"\-", @"\*\*", @"\*", @"\/\/", @"\/", @"\%", @"\&", @"\|", @"\^", @"\~", @"\(", @"\)" };
            _processors = new List<IProcessor>
            {
                new BinaryNumberParser(),
                new HexaNumberParser(),
                new OctalNumberParser(),
                new RomanNumberParser(),
                new PrefixedNumberParser()
            };
            StringBuilder regex = new StringBuilder();
            foreach (var @operator in _operators)
            {
                regex.AppendFormat("({0})|", @operator);
            }
            regex.Remove(regex.Length - 1, 1);
            _tokenizerregex = regex.ToString();
        }

        private string[] TokenizeInput(string input)
        {
            return Regex.Split(input, _tokenizerregex);
        }

        public string Process(string input)
        {
            var tokens = TokenizeInput(input);

            //foreach (var token in tokens)
            for (int i=0; i<tokens.Length; i++)
            {
                foreach (var processor in _processors)
                {
                    if (Regex.IsMatch(tokens[i], processor.PatternMatchRegex))
                    {
                        string temp;
                        if (processor.Process(tokens[i], out temp))
                        {
                            tokens[i] = temp;
                        }
                        else
                        {
                            throw new Exception("Parse error");
                        }
                    }
                }
            }

            return string.Join(" ", tokens);
        }

    }
}
