namespace Thingy.CalculatorCore.PreprocessorInternals
{
    internal class OctalNumberParser : NumberParser
    {
        public OctalNumberParser() : base(8)
        {
        }

        public override string PatternMatchRegex
        {
            get { return @"^(0|1|2|3|4|5|6|7){1,22}\:(OCT)"; }
        }
    }
}
