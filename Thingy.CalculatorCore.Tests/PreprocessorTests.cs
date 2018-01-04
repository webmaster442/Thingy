using NUnit.Framework;
using System.Collections.Generic;

namespace Thingy.CalculatorCore.Tests
{
    [TestFixture]
    public class PreprocessorTests
    {
        [TestCase("33 + 22", "33+22   ")]                    //white space test 1
        [TestCase("33 + 22", "33+22")]                       //white space test 2
        [TestCase("33 + 255 * 3", "33+FF:HEX*3")]            //Custom format parsing: HEX
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

            Preprocessor p = new Preprocessor(table);
            string output = p.Process(input);
            Assert.AreEqual(expected, output);
        }
    }
}
