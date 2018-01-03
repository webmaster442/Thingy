using NUnit.Framework;
using Thingy.CalculatorCore;

namespace Thingy.CalculatorCore.Tests
{
    [TestFixture]
    public class PreprocessorTests
    {
        [TestCase("33 + 22", "33+22   ", false)]                    //white space test 1
        [TestCase("33 + 22", "33+22", false)]                       //white space test 2
        [TestCase("33 + 255 * 3", "33+FF:HEX*3", false)]            //Custom format parsing: HEX
        [TestCase("33 + 255 * 3", "33+XXX:HEX*3", true)]            //Invalid Custom format parsing
        public void EnsureThatPreprocessorWorks(string expected, string input, bool throwsException)
        {
            Preprocessor p = new Preprocessor();
            if (throwsException)
            {
                Assert.Fail("Implement exception throwing");
            }
            else
            {
                string output = p.Process(input);
                Assert.AreEqual(expected, output);
            }
        }
    }
}
