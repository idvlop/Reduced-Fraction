using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;

namespace ReducedFraction
{
    [TestFixture]
    public class ReducedFraction_tests
    {
        public void AssertEqual(int expectedNumerator, int expectedDenominator, ReducedFraction actual)
        {
            Assert.That(!actual.IsNan);
            Assert.That(actual.Numerator, Is.EqualTo(expectedNumerator));
            Assert.That(actual.Denominator, Is.EqualTo(expectedDenominator));
        }

        #region INIT_TESTS
        [Test]
        public void InitializeSimpleRatioCorrectly()
        {
            AssertEqual(1, 2, new ReducedFraction(1, 2));
        }

        [Test]
        public void InitializeWithoutDenominator()
        {
            AssertEqual(4, 1, new ReducedFraction(4));
        }

        [Test]
        public void InitializeWithZeroDenominator()
        {
            Assert.That(new ReducedFraction(2, 0).IsNan);
        }

        [Test]
        public void BeCorrectWithZeroNumerator()
        {
            AssertEqual(0, 1, new ReducedFraction(0, 5));
        }

        [TestCase(1, 2, 2, 4)]
        [TestCase(-1, 2, -2, 4)]
        [TestCase(-1, 2, 2, -4)]
        [TestCase(1, 2, -2, -4)]
        [TestCase(1, 2, 1, 2)]
        [TestCase(1, 2, 8, 16)]
        [TestCase(2, 3, 10, 15)]
        [TestCase(4, 7, 16, 28)]
        [TestCase(3, 256, 12, 1024)]
        [TestCase(1, 1, 1, 1)]
        public void InitializeAndReduce(int expectedNum, int expectedDen, int num, int den)
        {
            AssertEqual(expectedNum, expectedDen, new ReducedFraction(num, den));
        }
        #endregion

        #region CONVERTATIONS_TESTS
        [Test]
        public void ConvertFromInt()
        {
            ReducedFraction r = 5;
            AssertEqual(5, 1, r);
        }

        [TestCase(0, 1, 0)]
        [TestCase(1, 1, 1)]
        [TestCase(2, 1, 2)]
        [TestCase(3, 1, 3)]
        [TestCase(2, 2, 1)]
        [TestCase(6, 3, 2)]
        [TestCase(12, 2, 6)]
        [TestCase(12, 3, 4)]
        [TestCase(12, 4, 3)]
        [TestCase(12, 6, 2)]
        [TestCase(12, 12, 1)]
        [TestCase(1000, 1, 1000)]
        public void ExplicitlyConvertToInt(int numerator, int denominator, int expectedValue)
        {
            int a = (int)new ReducedFraction(numerator, denominator);
            Assert.That(a, Is.EqualTo(expectedValue));
        }

        [TestCase(1, 2)]
        [TestCase(12, 5)]
        [TestCase(12, 10)]
        [TestCase(25, 8)]
        [TestCase(2, 3)]
        [TestCase(2, 4)]
        public void ExplicitlyConvertToIntAndFailsIfNonConvertible(int numerator, int denominator)
        {
            Assert.Catch<Exception>(() => { int a = (int)new ReducedFraction(numerator, denominator); });
        }
        #endregion

        #region OPERATIONS_RF_WITH_RF_TESTS
        [Test]
        public void Sum()
        {
            AssertEqual(1, 2, new ReducedFraction(1, 4) + new ReducedFraction(1, 4));
        }

        [Test]
        public void SumWithNan()
        {
            Assert.That((new ReducedFraction(1, 2) + new ReducedFraction(1, 0)).IsNan);
            Assert.That((new ReducedFraction(1, 0) + new ReducedFraction(1, 2)).IsNan);
        }

        [Test]
        public void Subtract()
        {
            AssertEqual(1, 4, new ReducedFraction(1, 2) - new ReducedFraction(1, 4));
        }

        [Test]
        public void SubtractWithNan()
        {
            Assert.That((new ReducedFraction(1, 2) - new ReducedFraction(1, 0)).IsNan);
            Assert.That((new ReducedFraction(1, 0) - new ReducedFraction(1, 2)).IsNan);
        }

        [Test]
        public void Multiply()
        {
            AssertEqual(-1, 4, new ReducedFraction(-1, 2) * new ReducedFraction(1, 2));
        }

        [Test]
        public void MultiplyWithNan()
        {
            Assert.That((new ReducedFraction(1, 2) * new ReducedFraction(1, 0)).IsNan);
            Assert.That((new ReducedFraction(1, 0) * new ReducedFraction(1, 2)).IsNan);
        }

        [Test]
        public void Divide()
        {
            AssertEqual(-1, 2, new ReducedFraction(1, 4) / new ReducedFraction(-1, 2));
        }

        [Test]
        public void DivideWithNan()
        {
            Assert.That((new ReducedFraction(1, 2) / new ReducedFraction(1, 0)).IsNan);
            Assert.That((new ReducedFraction(1, 0) / new ReducedFraction(1, 2)).IsNan);
        }

        [Test]
        public void DivideToZero()
        {
            Assert.That((new ReducedFraction(1, 2) / new ReducedFraction(0, 5)).IsNan);
        }

        [TestCase(1, 2, 0.5d)]
        [TestCase(10, 5, 2d)]
        [TestCase(-1, 5, -0.2d)]
        [TestCase(10, 0, double.NaN)]
        [TestCase(-10, 0, double.NaN)]
        [TestCase(0, 0, double.NaN)]
        public void ConvertToDouble(int numerator, int denominator, double expectedValue)
        {
            double v = new ReducedFraction(numerator, denominator);
            Assert.That(v, Is.EqualTo(expectedValue).Within(1e-7));
        }
        #endregion
        
        #region OPERATIONS_WITH_INT_TESTS
        [Test]
        public void SumWithInt()
        {
            AssertEqual(5, 4, 1 + new ReducedFraction(1, 4));
            AssertEqual(3, 4, 1 + new ReducedFraction(-1, 4));
            AssertEqual(-5, 4, -1 + new ReducedFraction(-1, 4));
            AssertEqual(-3, 4, -1 + new ReducedFraction(1, 4));
            AssertEqual(-1, 4, 0 + new ReducedFraction(-1, 4));

            AssertEqual(5, 4, new ReducedFraction(1, 4) + 1);
            AssertEqual(3, 4, new ReducedFraction(-1, 4) + 1);
            AssertEqual(-5, 4, new ReducedFraction(-1, 4) + -1);
            AssertEqual(-3, 4, new ReducedFraction(1, 4) + -1);
            AssertEqual(-1, 4, new ReducedFraction(-1, 4) + 0);
        }

        [Test]
        public void SumNanWithInt()
        {
            Assert.That((1 + new ReducedFraction(1, 0)).IsNan);
            Assert.That((new ReducedFraction(1, 0) + 1).IsNan);
        }

        [Test]
        public void SubtractWithInt()
        {
            AssertEqual(3, 4, 1 - new ReducedFraction(1, 4));
            AssertEqual(-5, 4, -1 - new ReducedFraction(1, 4));
            AssertEqual(-1, 4, 0 - new ReducedFraction(1, 4));

            AssertEqual(1, 4,  new ReducedFraction(5, 4) - 1);
            AssertEqual(-3, 4, new ReducedFraction(5, 4) - 2);
            AssertEqual(5, 4, new ReducedFraction(5, 4) - 0);
        }

        [Test]
        public void SubtractNanWithInt()
        {
            Assert.That((1 - new ReducedFraction(1, 0)).IsNan);
            Assert.That((new ReducedFraction(1, 0) - 1).IsNan);
        }

        [Test]
        public void MultiplyWithInt()
        {
            AssertEqual(5, 2, new ReducedFraction(1, 2) * 5);
            AssertEqual(0, 1, new ReducedFraction(1, 2) * 0);
            AssertEqual(5, 2, new ReducedFraction(-1, 2) * -5);
            AssertEqual(-1, 1, new ReducedFraction(-1, 2) * 2);
            AssertEqual(-5, 1, new ReducedFraction(1, 2) * -10);

            AssertEqual(5, 2, 5 * new ReducedFraction(1, 2));
            AssertEqual(0, 1, 0 * new ReducedFraction(1, 2));
            AssertEqual(5, 2, -5 * new ReducedFraction(-1, 2));
            AssertEqual(-1, 1, 2 * new ReducedFraction(-1, 2));
            AssertEqual(-5, 1, -10 * new ReducedFraction(1, 2));
        }

        [Test]
        public void MultiplyNanWithInt()
        {
 
            Assert.That((new ReducedFraction(1, 0) * 6).IsNan);
            Assert.That((6 * new ReducedFraction(1, 0)).IsNan);
        }

        [Test]
        public void DivideWithInt()
        {
            AssertEqual(1, 20, new ReducedFraction(1, 4) / 5);
            AssertEqual(1, 1, new ReducedFraction(20, 4) / 5);
            AssertEqual(-1, 20, new ReducedFraction(-1, 4) / 5);

            AssertEqual(20, 1, 5 / new ReducedFraction(1, 4));
            AssertEqual(1, 1, 5 / new ReducedFraction(20, 4));
            AssertEqual(-20, 1, 5 / new ReducedFraction(-1, 4));
        }

        [Test]
        public void DivideNanWithInt()
        {
            Assert.That((5 / new ReducedFraction(1, 0)).IsNan);
        }

        [Test]
        public void DivideToZeroWithInt()
        {
            Assert.That((new ReducedFraction(1, 2) / 0).IsNan);
            Assert.That(!(0 / new ReducedFraction(1, 2)).IsNan);
            Assert.That((5 / new ReducedFraction(0, 1)).IsNan);
        }
        #endregion
    }
}
