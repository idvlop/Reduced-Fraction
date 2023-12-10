using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.RationalNumbers
{
    public struct Rational
    {
        public Rational(int numerator, int denominator = 1)
        {
            Numerator = 0;
            Denominator = 0;
            IsNan = false;

            Numerator = numerator;
            Denominator = numerator == 0 ? 1 : denominator;
            IsNan = Denominator == 0;
        }

        public int Numerator { get; set; }
        public int Denominator { get; set; }
        public bool IsNan { get; }

        public static Rational operator +(Rational left, Rational right)
        {
            if (left.IsNan || right.IsNan)
                throw new ArgumentException();

            var gcd = GCD(left.Denominator, right.Denominator);
            var lcm1 = LCM(gcd, left.Numerator);
            var lcm2 = LCM(gcd, right.Numerator);

            return new Rational(left.Numerator * lcm1 + right.Numerator * lcm2, gcd);
        }

        public static Rational operator *(Rational left, Rational right)
        {
            if (left.IsNan || right.IsNan)
                throw new ArgumentException();

            var newNom = left.Numerator * right.Numerator;
            var newDen = left.Denominator * right.Denominator;

            return new Rational(newNom, newDen);
        }

        public static Rational operator -(Rational left, Rational right)
        {
            if (left.IsNan || right.IsNan)
                throw new ArgumentException();

            var gcd = GCD(left.Denominator, right.Denominator);
            var lcm1 = LCM(gcd, left.Numerator);
            var lcm2 = LCM(gcd, right.Numerator);

            return new Rational(left.Numerator * lcm1 - right.Numerator * lcm2, gcd);
        }

        public static Rational operator /(Rational left, Rational right)
        {
            if (left.IsNan || right.IsNan)
                throw new ArgumentException();

            var newNom = left.Numerator * right.Denominator;
            var newDen = left.Denominator * right.Numerator;

            return new Rational(newNom, newDen);
        }

        public static implicit operator double(Rational rational) => rational.IsNan ? double.NaN : (double) rational.Numerator / rational.Denominator;

        public static implicit operator Rational(int x) => new Rational(x);

        // НОК
        private static int GCD(int a, int b)
        {
            return b != 0 ? GCD(b, a % b) : a; ;
        }

        // НОД
        private static int LCM(int a, int b)
        {
            return a / GCD(a, b) * b;
        }
    }
}
