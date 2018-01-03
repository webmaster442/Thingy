using NUnit.Framework;
using Thingy.CalculatorCore.PreprocessorInternals;

namespace Thingy.CalculatorCore.Tests
{
    [TestFixture]
    public class OctalNumberParserTests
    {
        [TestCase("128", "200:OCT", true)]
        [TestCase("37296", "110660:OCT", true)]
        [TestCase("110668:OCT", "110668:OCT", false)]
        [TestCase(":OCT", ":OCT", false)]
        [TestCase("asd:OCT", "asd:OCT", false)]
        public void EnsureThatOctalNumberParserWorks(string expected, string input, bool valid)
        {
            NumberParser parser = new OctalNumberParser();

            string output;
            bool result = parser.Process(input, out output);

            Assert.AreEqual(expected, output);
            Assert.AreEqual(valid, result);
        }
    }
}
