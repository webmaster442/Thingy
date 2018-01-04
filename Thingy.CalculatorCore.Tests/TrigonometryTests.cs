using AppLib.Maths;
using NUnit.Framework;

namespace Thingy.CalculatorCore.Tests
{
    [TestFixture]
    public class TrigonometryTests
    {
        [Test]
        public void EnsureThatSinWorks()
        {
            Trigonometry.Mode = TrigonometryMode.DEG;
            Assert.AreEqual(1, Trigonometry.Sin(90));
            Assert.AreEqual(0, Trigonometry.Sin(0));
        }

        [Test]
        public void EnsureThatCosWorks()
        {
            Trigonometry.Mode = TrigonometryMode.DEG;
            Assert.AreEqual(0, Trigonometry.Cos(90));
            Assert.AreEqual(1, Trigonometry.Cos(0));
        }
    }
}
