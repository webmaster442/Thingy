using NUnit.Framework;
using System.Collections.Generic;
using System.Globalization;

namespace Thingy.CalculatorCore.Tests
{
    [TestFixture]
    public class PreprocessorTests
    {
        [TestCase("33 + 22", "33+22   ")]                    //white space test 1
        [TestCase("33 + 22", "33+22")]                       //white space test 2
        [TestCase("33 + 255 * 3", "33+FF:HEX*3")]            //Custom format parsing: HEX
        [TestCase("33 + 255 * 3", "33+FF:S16*3")]            //Custom format parsing: S16
        [TestCase("33 + XXX:HEX * 3", "33+XXX:HEX*3")]       //Invalid Custom format parsing
        [TestCase("ClassName.Sin ( 90 )", "sin(90)")]        //Function name replacing
        [TestCase("ClassName.Sin ( 255 )", "sin(FF:HEX)")]   //Function name replacing with custom format
        public void EnsureThatPreprocessorWorks(string expected, string input)
        {
            Dictionary<string, string> table = new Dictionary<string, string>
            {
                { "sin", "ClassName.Sin" },
                { "cos", "ClassName.Cos" }
            };

            Preprocessor p = new Preprocessor(table, null);
            string output = p.Process(input);
            Assert.AreEqual(expected, output);
        }

        [TestCase(2.7182818284590452353602874713526624977572470937000d, "C:e")]
        [TestCase(3.1415926535897932384626433832795028841971693993751d, "C:Pi")]
        public void EnsureThatPreprocessorProcessesConstants(double expected, string input)
        {
            var db = new Constants.ConstantDB();

            Preprocessor p = new Preprocessor(null, db);
            string output = p.Process(input);
            double d = System.Convert.ToDouble(output, new CultureInfo("en-US"));
            Assert.AreEqual(expected, d, 0.0001);
        }
    }
}
