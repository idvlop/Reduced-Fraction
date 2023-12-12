using System;
using System.Security.Cryptography;

namespace ReducedFraction
{
    /// <summary>Reduced Fraction</summary>
    /// <remarks>*Can be either proper or improper. Can not be a mixed fraction.</remarks>
    public struct ReducedFraction
    {
        /// <summary>Reduced Fraction</summary>
        /// <remarks>*Can be either proper or improper. Can not be a mixed fraction.</remarks>
        public ReducedFraction(int numerator, int denominator = 1)
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

        /// <summary>Fraction numerator</summary>
        public int Numerator { get; }
        /// <summary>Fraction denominator</summary>
        public int Denominator { get; }
        /// <summary>Is fraction not a number?</summary>
        public bool IsNan { get; }

        /// <summary>Represents a value that is not a number (NaN)</summary>
        public static ReducedFraction NaN { get => new ReducedFraction(0, 0); }

        public override string ToString() => $"{Numerator} / {Denominator}";

        #region METHODS
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

        #region OPERATIONS_RF_RF
        /// <summary>Text for "left.IsNan && right.IsNan" ArgumentException in operations</summary>
        private const string TextForBothNanArgException = "Both arguments can not be NaN.";

        /// <summary>ReducedFraction * ReducedFraction</summary>
        public static ReducedFraction operator *(ReducedFraction left, ReducedFraction right)
        {
            if (left.IsNan && right.IsNan)
                throw new ArgumentException(TextForBothNanArgException);
            else if (right.IsNan || left.IsNan)
                return NaN;

            var newNumerator = left.Numerator * right.Numerator;
            var newDenominator = left.Denominator * right.Denominator;

            return newDenominator == 0
                ? NaN
                : new ReducedFraction(newNumerator, newDenominator);
        }

        /// <summary>ReducedFraction / ReducedFraction</summary>
        public static ReducedFraction operator /(ReducedFraction left, ReducedFraction right)
        {
            if (left.IsNan && right.IsNan)
                throw new ArgumentException(TextForBothNanArgException);
            else if (right.IsNan || left.IsNan)
                return NaN;

            var newNumerator = left.Numerator * right.Denominator;
            var newDenominator = left.Denominator * right.Numerator;

            return newDenominator == 0
                ? NaN
                : new ReducedFraction(newNumerator, newDenominator);
        }

        /// <summary>ReducedFraction + ReducedFraction</summary>
        public static ReducedFraction operator +(ReducedFraction left, ReducedFraction right)
        {
            if (left.IsNan && right.IsNan)
                throw new ArgumentException(TextForBothNanArgException);
            else if (right.IsNan || left.IsNan)
                return NaN;

            var lcm = GetLCM(left.Denominator, right.Denominator);
            var multForLeftNum = lcm / left.Denominator;
            var multForRightNum = lcm / right.Denominator;

            var newNumerator = left.Numerator * multForLeftNum + right.Numerator * multForRightNum;

            return new ReducedFraction(newNumerator, lcm);
        }

        /// <summary>ReducedFraction - ReducedFraction</summary>
        public static ReducedFraction operator -(ReducedFraction left, ReducedFraction right)
        {
            if (left.IsNan && right.IsNan)
                throw new ArgumentException(TextForBothNanArgException);
            else if (right.IsNan || left.IsNan)
                return NaN;

            var lcm = GetLCM(left.Denominator, right.Denominator);
            var multForLeftNum = lcm / left.Denominator;
            var multForRightNum = lcm / right.Denominator;

            var newNumerator = left.Numerator * multForLeftNum - right.Numerator * multForRightNum;

            return new ReducedFraction(newNumerator, lcm);
        }
        #endregion

        #region OPERATIONS_RF_INT
        /// <summary>ReducedFraction * int</summary>
        public static ReducedFraction operator *(ReducedFraction rational, int integer)
        {
            if (rational.IsNan)
                return NaN;

            var newNumerator = rational.Numerator * integer;

            return new ReducedFraction(newNumerator, rational.Denominator);
        }

        /// <summary>int * ReducedFraction</summary>
        public static ReducedFraction operator *(int integer, ReducedFraction rational) => rational * integer;


        /// <summary>ReducedFraction / int</summary>
        public static ReducedFraction operator /(ReducedFraction rational, int integer)
        {
            if (rational.IsNan || integer == 0)
                return NaN;

            var newDenominator = rational.Denominator * integer;

            return new ReducedFraction(rational.Numerator, newDenominator);
        }

        /// <summary>int / ReducedFraction</summary>
        public static ReducedFraction operator /(int integer, ReducedFraction rational)
        {
            if (rational.IsNan)
                return NaN;

            var leftRational = new ReducedFraction(integer);

            return leftRational / rational;

        }

        /// <summary>ReducedFraction + int</summary>
        public static ReducedFraction operator +(ReducedFraction rational, int integer)
        {
            if (rational.IsNan)
                return NaN;

            var rightRational = new ReducedFraction(integer);

            return rational + rightRational;
        }

        /// <summary>int + ReducedFraction</summary>
        public static ReducedFraction operator +(int integer, ReducedFraction rational) => rational + integer;


        /// <summary>ReducedFraction - int</summary>
        public static ReducedFraction operator -(ReducedFraction rational, int integer)
        {
            if (rational.IsNan)
                return NaN;

            var rightRational = new ReducedFraction(integer);

            return rational - rightRational;
        }

        /// <summary>int - ReducedFraction</summary>
        public static ReducedFraction operator -(int integer, ReducedFraction rational)
        {
            if (rational.IsNan)
                return NaN;

            var leftRational = new ReducedFraction(integer);

            return leftRational - rational;
        }
        #endregion

        #region CONVERTATIONS
        /// <summary>Implicitly convert int to ReducedFraction</summary>
        public static implicit operator ReducedFraction(int x) => new ReducedFraction(x);

        /// <summary>Implicitly convert ReducedFraction to double</summary>
        public static implicit operator double(ReducedFraction rational) =>
            rational.IsNan
            ? double.NaN
            : rational.Numerator / (double)rational.Denominator;

        /// <summary>Explicitly convert ReducedFraction to int</summary>
        public static explicit operator int(ReducedFraction rational)
        {
            if (rational.IsNan)
                throw new Exception("Can not convert NaN to int.");

            rational = new ReducedFraction(rational.Numerator, rational.Denominator);

            return rational.Denominator == 1
                ? rational.Numerator
                : throw new Exception("The number is not an integer.");
        }
        #endregion
    }
}
