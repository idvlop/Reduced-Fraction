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
            Denominator = numerator == 0 && denominator != 0 ? 1 : denominator;
            Numerator = numerator;
            IsNan = denominator == 0;
            if (!IsNan)
            {
                var gcd = GCD(Numerator, Denominator);
                if(gcd != 1)
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
            //var isInproper = !IsNan && numerator != 0 && denominator != 1 && denominator != -1 && numerator != 1 && numerator != -1;
            //if (isInproper)
            //{
            //    var rat = ToProper(numerator, denominator);
            //    Numerator = rat.Numerator;
            //    Denominator = rat.Denominator;
            //}
        }

        public int Numerator { get; set; }
        public int Denominator { get; set; }
        public bool IsNan { get; }

        public static Rational operator +(Rational left, Rational right)
        {
            if (left.IsNan && right.IsNan)
                throw new ArgumentException();
            else if (right.IsNan || left.IsNan)
                return GetNan;

            var lcm = LCM(left.Denominator, right.Denominator);
            var gcd1 = GCD(lcm, left.Numerator);
            var gcd2 = GCD(lcm, right.Numerator);

            var newNumerator = left.Numerator * gcd1 + right.Numerator * gcd2;

            return ToProper(newNumerator, lcm);
        }

        public static Rational operator *(Rational left, Rational right)
        {
            if (left.IsNan && right.IsNan)
                throw new ArgumentException();
            else if (right.IsNan || left.IsNan)
                return GetNan;

            var newNom = left.Numerator * right.Numerator;
            var newDen = left.Denominator * right.Denominator;

            return newDen == 0 ? GetNan : ToProper(newNom, newDen);
        }

        public static Rational operator -(Rational left, Rational right)
        {
            if (left.IsNan && right.IsNan)
                throw new ArgumentException();
            else if (right.IsNan || left.IsNan)
                return GetNan;

            var lcm = LCM(left.Denominator, right.Denominator);
            var multForLeftNum = lcm / left.Denominator;
            var multForRightNum = lcm / right.Denominator;

            var newNumerator = left.Numerator * multForLeftNum - right.Numerator * multForRightNum;

            return ToProper(newNumerator, lcm);
        }

        public static Rational operator /(Rational left, Rational right)
        {
            if (left.IsNan && right.IsNan)
                throw new ArgumentException();
            else if (right.IsNan || left.IsNan)
                return GetNan;

            var newNom = left.Numerator * right.Denominator;
            var newDen = left.Denominator * right.Numerator;

            if (newDen < 0)
            {
                newNom = -newNom;
                newDen = -newDen;
            }
            return newDen == 0 ? GetNan : ToProper(newNom, newDen);
        }

        public static implicit operator double(Rational rational) => 
            rational.IsNan
            ? double.NaN
            : rational.Numerator / (double) rational.Denominator;

        public static explicit operator int(Rational rational)
        {
            if(rational.IsNan)
                throw new Exception();

            rational = ToProper(rational.Numerator, rational.Denominator);
            
            return rational.Denominator == 1 ? rational.Numerator :
                throw new Exception();
        }

        public static implicit operator Rational(int x) => new Rational(x);

        // НОД - Наибольший общий делитель
        // GCD - Greatest common divisor
        private static int GCD(int a, int b)
        {
            //if (a == 1 || b == 1 )
            //    return 1;

            //while (a != 0 && b != 0)
            //{
            //    if (a > b)
            //        a %= b;
            //    else
            //        b %= a;
            //}

            //return a | b;

            var res = Math.Abs((a * b) / LCM(a, b));
            return res == 0 ? 1 : res;
        }

        // НОК - Наименьшее общее кратное
        // LCM - Least common multiple
        private static int LCM(int a, int b)
        {
            int num1, num2;
            a = Math.Abs(a);
            b = Math.Abs(b);
            if (a > b)
            {
                num1 = a; num2 = b;
            }
            else
            {
                num1 = b; num2 = a;
            }

            for (int i = 1; i < num2; i++)
            {
                int mult = num1 * i;
                if (mult % num2 == 0)
                {
                    return mult;
                }
            }

            var res = num1 * num2;
            return res == 0 ? 1 : res;
        }

        private static Rational ToProper(int a, int b)
        {
            var gcd = Math.Abs(GCD(a, b));
            return new Rational(a / gcd, b / gcd);
        }

        private static Rational GetNan = new Rational(0, 0);
    }
}
