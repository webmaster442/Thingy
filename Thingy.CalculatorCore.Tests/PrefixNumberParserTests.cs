using NUnit.Framework;
using Thingy.CalculatorCore.PreprocessorInternals;

namespace Thingy.CalculatorCore.Tests
{
    [TestFixture]
    public class PrefixNumberParserTests
    {
        [TestCase("0.128", "128:m", true)]
        [TestCase("1E-08", "10:n", true)]
        [TestCase("2500", "2.5:k", true)]
        [TestCase("0.1", "1:d", true)]
        [TestCase(":u", ":u", false)]
        [TestCase(":M", ":M", false)]
        public void EnsureThatPrefixNumberParserWorks(string expected, string input, bool valid)
        {
            PrefixedNumberParser parser = new PrefixedNumberParser();

            string output;
            bool result = parser.Process(input, out output);

            Assert.AreEqual(expected, output);
            Assert.AreEqual(valid, result);
        }
    }
}
