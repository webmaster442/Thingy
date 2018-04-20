using NUnit.Framework;
using Thingy.CalculatorCore.PreprocessorInternals;

namespace Thingy.CalculatorCore.Tests
{
    [TestFixture]
    public class BinaryNumberParserTests
    {
        [TestCase("128", "10000000:BIN", true)]
        [TestCase("37296", "1001000110110000:BIN", true)]
        [TestCase("1001000110110002:BIN", "1001000110110002:BIN", false)]
        [TestCase(":BIN", ":BIN", false)]
        [TestCase("asd:BIN", "asd:BIN", false)]
        public void EnsureThatBinaryNumberParserWorks(string expected, string input, bool valid)
        {
            BinaryNumberParser parser = new BinaryNumberParser();

            string output;
            bool result = parser.Process(input, out output);

            Assert.AreEqual(expected, output);
            Assert.AreEqual(valid, result);
        }
    }
}
