using System;

namespace Incapsulation.RationalNumbers
{
    /// <summary>Reduced Fraction</summary>
    /// <remarks>*Can be either proper or improper. Can not be a mixed fraction.</remarks>
    public struct Rational
    {
        /// <summary>Reduced Fraction</summary>
        /// <remarks>*Can be either proper or improper. Can not be a mixed fraction.</remarks>
        public Rational(int numerator, int denominator = 1)
        {
            // init struct properties
            Numerator = 0;
            Denominator = 0;
            IsNan = false;
            // define struct properties
            Numerator = numerator;
            Denominator = numerator == 0 && denominator != 0
                ? 1
                : denominator;
            IsNan = denominator == 0;
            // reducing
            if (!IsNan)
            {
                var gcd = GetGCD(Numerator, Denominator);
                if (gcd != 1)
                {
                    Denominator /= gcd;
                    Numerator /= gcd;
                }
                if (Denominator < 0)
                {
                    Numerator = -Numerator;
                    Denominator = -Denominator;
                }
            }
        }

        public int Numerator { get; set; }
        public int Denominator { get; set; }
        public bool IsNan { get; }

        /// <summary>Represents a value that is not a number (NaN)</summary>
        public static Rational NaN = new Rational(0, 0);

        #region methods
        /// <summary>Get Greatest common divisor</summary>
        private static int GetGCD(int a, int b)
        {
            var gcd = Math.Abs((a * b) / GetLCM(a, b));
            return gcd == 0 ? 1 : gcd;
        }

        /// <summary>Get Least common multiple</summary>
        private static int GetLCM(int a, int b)
        {
            int num1, num2;
            a = Math.Abs(a);
            b = Math.Abs(b);

            if (a > b)
            {
                num1 = a;
                num2 = b;
            }
            else
            {
                num1 = b;
                num2 = a;
            }

            for (int i = 1; i < num2; i++)
            {
                int mult = num1 * i;
                if (mult % num2 == 0)
                    return mult;
            }

            var lcm = num1 * num2;
            return lcm == 0 ? 1 : lcm;
        }
        #endregion

        #region OPERATIONS
        private const string TextForBothNanArgException = "Both arguments can not be NaN.";

        public static Rational operator *(Rational left, Rational right)
        {
            if (left.IsNan && right.IsNan)
                throw new ArgumentException(TextForBothNanArgException);
            else if (right.IsNan || left.IsNan)
                return NaN;

            var newNumerator = left.Numerator * right.Numerator;
            var newDenominator = left.Denominator * right.Denominator;

            return newDenominator == 0
                ? NaN
                : new Rational(newNumerator, newDenominator);
        }

        public static Rational operator /(Rational left, Rational right)
        {
            if (left.IsNan && right.IsNan)
                throw new ArgumentException(TextForBothNanArgException);
            else if (right.IsNan || left.IsNan)
                return NaN;

            var newNumerator = left.Numerator * right.Denominator;
            var newDenominator = left.Denominator * right.Numerator;

            return newDenominator == 0
                ? NaN
                : new Rational(newNumerator, newDenominator);
        }

        public static Rational operator +(Rational left, Rational right)
        {
            if (left.IsNan && right.IsNan)
                throw new ArgumentException(TextForBothNanArgException);
            else if (right.IsNan || left.IsNan)
                return NaN;

            var lcm = GetLCM(left.Denominator, right.Denominator);
            var gcd1 = GetGCD(lcm, left.Numerator);
            var gcd2 = GetGCD(lcm, right.Numerator);

            var newNumerator = left.Numerator * gcd1 + right.Numerator * gcd2;

            return new Rational(newNumerator, lcm);
        }

        public static Rational operator -(Rational left, Rational right)
        {
            if (left.IsNan && right.IsNan)
                throw new ArgumentException(TextForBothNanArgException);
            else if (right.IsNan || left.IsNan)
                return NaN;

            var lcm = GetLCM(left.Denominator, right.Denominator);
            var multForLeftNum = lcm / left.Denominator;
            var multForRightNum = lcm / right.Denominator;

            var newNumerator = left.Numerator * multForLeftNum - right.Numerator * multForRightNum;

            return new Rational(newNumerator, lcm);
        }
        #endregion

        #region CONVERTATION
        /// <summary>Implicitly convert int to Rational</summary>
        public static implicit operator Rational(int x) => new Rational(x);

        /// <summary>Implicitly convert Rational to double</summary>
        public static implicit operator double(Rational rational) =>
            rational.IsNan
            ? double.NaN
            : rational.Numerator / (double)rational.Denominator;

        /// <summary>Explicitly convert Rational to int</summary>
        public static explicit operator int(Rational rational)
        {
            if (rational.IsNan)
                throw new Exception("Can not convert NaN to int.");

            rational = new Rational(rational.Numerator, rational.Denominator);

            return rational.Denominator == 1
                ? rational.Numerator
                : throw new Exception("The number is not an integer.");
        }
        #endregion
    }
}
