namespace SpaceBattle.Lib.Test;

public class AngleTest
{
    [Fact]
    public void DivizionByZeroException()
    {
        Assert.Throws<Exception>(() => new Angle(99, 0));
    }

    [Fact]
    public void AngleSumTestPositive()
    {
        Angle a = new Angle(45, 1);
        Angle b = new Angle(90, 2);
        Angle c = a + b;
        Assert.Equal(new Angle(90, 1), c);
    }

    [Fact]
    public void AngleSumTestNegative()
    {
        Angle a = new Angle(45, 1);
        Angle b = new Angle(90, 2);
        Assert.False(a + b == new Angle(90, 2));
    }

    [Fact]
    public void AngleNODTestPositive()
    {
        int nod = Angle.NOD(4, 5);
        Assert.Equal(1, nod);
    }

    [Fact]
    public void AngleTestComparingHashCodePositive()
    {
        Angle a = new Angle(45, 1);
        Angle b = new Angle(90, 2);
        Assert.True(a.GetHashCode() == b.GetHashCode());
    }

    [Fact]
    public void AngleTestComparingHashCodeNegative()
    {
        Angle a = new Angle(42, 1);
        Angle b = new Angle(90, 2);
        Assert.True(a.GetHashCode() != b.GetHashCode());
    }

    [Fact]
    public void AngleTestEqualNegative()
    {
        Angle a = new Angle(45, 1);
        Angle b = new Angle(90, 2);
        Assert.False(a != b);
    }

    [Fact]
    public void AngleTestEqualNegativeDifferentTypes()
    {
        Angle a = new Angle(45, 1);
        Assert.False(a.Equals("String"));
    }
}

