using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Thingy.CalculatorCore.PreprocessorInternals;

namespace Thingy.CalculatorCore
{
    public class Preprocessor
    {
        private readonly char[] _operators;
        private readonly List<IProcessor> _processors;

        public Preprocessor()
        {
            _operators = new char[] { '+', '-', '*', '%', '&', '|', '^', '~', '(', ')' };
            _processors = new List<IProcessor>();
            _processors.Add(new BinaryNumberParser());
            _processors.Add(new HexaNumberParser());
            _processors.Add(new OctalNumberParser());
            _processors.Add(new RomanNumberParser());
        }

        private IList<string> TokenizeExpression(string expr)
        {
            var buffer = string.Empty;
            var ret = new List<string>();
            expr = expr.Replace(" ", "");
            foreach (var c in expr)
            {
                if (_operators.Contains(c))
                {
                    if (buffer.Length > 0) ret.Add(buffer);
                    ret.Add(c.ToString(CultureInfo.InvariantCulture));
                    buffer = string.Empty;
                }
                else
                {
                    buffer += c;
                }
            }

            if (buffer.Length > 0)
            {
                ret.Add(buffer);
            }

            return ret;
        }

        public string Process(string input)
        {
            var tokens = TokenizeExpression(input);

            //foreach (var token in tokens)
            for (int i=0; i<tokens.Count; i++)
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
