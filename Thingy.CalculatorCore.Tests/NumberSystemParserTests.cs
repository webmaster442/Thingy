using NUnit.Framework;
using Thingy.CalculatorCore.PreprocessorInternals;

namespace Thingy.CalculatorCore.Tests
{
    [TestFixture]
    public class NumberSystemParserTests
    {
        [TestCase("128", "10000000:S2", true)]
        [TestCase("37296", "91B0:S16", true)]
        [TestCase("37296", "110660:S8", true)]
        [TestCase(":S99", ":S99", false)]
        [TestCase(":S2", ":S2", false)]
        public void EnsureThatCustomNumberSystemParserWorks(string expected, string input, bool valid)
        {
            CustomNumberSystemParser parser = new CustomNumberSystemParser();

            string output;
            bool result = parser.Process(input, out output);

            Assert.AreEqual(expected, output);
            Assert.AreEqual(valid, result);
        }
    }
}
