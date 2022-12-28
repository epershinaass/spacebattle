namespace SpaceBattle
{
    public class Angle
    {
        public int Numerator { get; set; }
        public int Denominator { get; set; }

        public Angle(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                throw new Exception();
            }

            int nod = NOD(numerator, denominator);
            this.Numerator = numerator / nod;
            this.Denominator = denominator / nod;
        }
        private static int GCD(int x, int y)
        {
            while (x != y)
            {
                if (x > y)
                    x = x - y;
                else
                    y = y - x;
            }
            return x;
        }

        public static Angle operator +(Angle a, Angle b)
        {
            int num = a.Numerator * b.Denominator + b.Numerator * a.Denominator;
            int den = a.Denominator * b.Denominator;
            int nod = NOD(num, den);
            return new Angle(num / nod, den / nod);
        }

        public static bool operator ==(Angle a, Angle b) => (a.Numerator == b.Numerator) && (a.Denominator == b.Denominator);

        public static bool operator !=(Angle a, Angle b) => !(a == b);

        public override bool Equals(object? obj) => obj is Angle a && this.Numerator == a.Numerator && this.Denominator == a.Denominator;

        public override int GetHashCode() => ((this.Numerator + this.Denominator).ToString()).GetHashCode();
    }
}
