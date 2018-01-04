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
        private readonly IDictionary<string, string> _replacetable;

        public Preprocessor(IDictionary<string, string> replcacetable)
        {
            _replacetable = replcacetable;
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

            for (int i=0; i<tokens.Length; i++)
            {
                if (_replacetable.Keys.Contains(tokens[i]))
                {
                    //replace it if it's a recognized function
                    tokens[i] = _replacetable[tokens[i]];
                }
                else
                {
                    //only run preprocessor modules, if it's not a function
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
            }

            return string.Join(" ", tokens).Trim();
        }

    }
}
