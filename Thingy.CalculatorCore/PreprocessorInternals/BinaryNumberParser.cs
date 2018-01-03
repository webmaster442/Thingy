namespace Thingy.CalculatorCore.PreprocessorInternals
{
    internal class BinaryNumberParser : NumberParser
    {
        public BinaryNumberParser() : base(2)
        {
        }

        public override string PatternMatchRegex
        {
            get { return @"^[01]{1,64}\:(BIN)"; }
        }
    }
}