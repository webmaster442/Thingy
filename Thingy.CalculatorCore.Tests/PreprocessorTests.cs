using NUnit.Framework;

namespace Thingy.CalculatorCore.Tests
{
    [TestFixture]
    public class PreprocessorTests
    {
        [TestCase("33 + 22", "33+22   ")]                    //white space test 1
        [TestCase("33 + 22", "33+22")]                       //white space test 2
        [TestCase("33 + 255 * 3", "33+FF:HEX*3")]            //Custom format parsing: HEX
        [TestCase("33 + XXX:HEX * 3", "33+XXX:HEX*3")]       //Invalid Custom format parsing
        public void EnsureThatPreprocessorWorks(string expected, string input)
        {
            Preprocessor p = new Preprocessor();
            string output = p.Process(input);
            Assert.AreEqual(expected, output);
        }
    }
}
