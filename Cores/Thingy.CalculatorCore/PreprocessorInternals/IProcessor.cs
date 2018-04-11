namespace Thingy.CalculatorCore.PreprocessorInternals
{
    internal interface IProcessor
    {
        string PatternMatchRegex { get;}
        bool Process(string input, out string output);
    }
}
