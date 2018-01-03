using NUnit.Framework;
using Thingy.CalculatorCore.PreprocessorInternals;

namespace Thingy.CalculatorCore.Tests
{
    [TestFixture]
    public class HexaNumberParserTests
    {
        [TestCase("128", "80:HEX", true)]
        [TestCase("37296", "91B0:HEX", true)]
        [TestCase(":HEX", ":HEX", false)]
        [TestCase("asd:HEX", "asd:HEX", false)]
        public void EnsureThatHexaNumberParserWorks(string expected, string input, bool valid)
        {
            NumberParser parser = new HexaNumberParser();

            string output;
            bool result = parser.Process(input, out output);

            Assert.AreEqual(expected, output);
            Assert.AreEqual(valid, result);
        }
    }
}
