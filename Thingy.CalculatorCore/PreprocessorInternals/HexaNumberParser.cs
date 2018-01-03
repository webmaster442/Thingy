namespace Thingy.CalculatorCore.PreprocessorInternals
{
    internal class HexaNumberParser : NumberParser
    {
        public HexaNumberParser() : base(16)
        {
        }

        public override string PatternMatchRegex
        {
            get { return @"^[0-9A-Fa-f]{1,16}\:(HEX)"; }
        }
    }
}
