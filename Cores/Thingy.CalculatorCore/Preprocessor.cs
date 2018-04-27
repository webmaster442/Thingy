using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Thingy.CalculatorCore.Constants;
using Thingy.CalculatorCore.FunctionCaching;
using Thingy.CalculatorCore.PreprocessorInternals;

namespace Thingy.CalculatorCore
{
    public class Preprocessor
    {
        private readonly IDictionary<string, FunctionInformation> _functionReplaceTable;
        private readonly string[] _operators;
        private readonly List<IProcessor> _processors;
        private readonly string _tokenizerregex;

        private string[] TokenizeInput(string input)
        {
            return Regex.Split(input, _tokenizerregex);
        }

        public Preprocessor(IDictionary<string, FunctionInformation> functionReplcaceTable, IConstantDB constantDB)
        {
            _functionReplaceTable = functionReplcaceTable;
            _operators = new string[] { @"\+", @"\-", @"\*\*", @"\*", @"\/\/", @"\/", @"\%", @"\&", @"\|", @"\^", @"\~", @"\(", @"\)", @"\," };
            _processors = new List<IProcessor>
            {
                new BinaryNumberParser(),
                new HexaNumberParser(),
                new OctalNumberParser(),
                new RomanNumberParser(),
                new PrefixedNumberParser(),
                new CustomNumberSystemParser(),
            };

            if (constantDB != null)
            {
                _processors.Add(new ConstantParser(constantDB));
            }

            StringBuilder regex = new StringBuilder();
            foreach (var @operator in _operators)
            {
                regex.AppendFormat("({0})|", @operator);
            }
            regex.Remove(regex.Length - 1, 1);
            _tokenizerregex = regex.ToString();
        }

        public string Process(string input)
        {
            var tokens = TokenizeInput(input);

            for (int i = 0; i < tokens.Length; i++)
            {
                if (_functionReplaceTable != null &&
                    _functionReplaceTable.Keys.Contains(tokens[i]))
                {
                    //replace it if it's a recognized function
                    tokens[i] = _functionReplaceTable[tokens[i]].FullName;
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
