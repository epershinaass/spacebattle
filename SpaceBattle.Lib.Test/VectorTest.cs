namespace SpaceBattle.Lib.Test;

public class VectorTest
{
    [Fact]
    public void VectorSumTestPositive()
    {
        Vector a = new Vector(5, 5);
        Vector b = new Vector(6, 15);
        Assert.Equal(new Vector(11, 20), a + b);
    }

    [Fact]
    public void VectorSumTestNegative()
    {
        Vector a = new Vector(5, 5);
        Vector b = new Vector(6, 15, 2);
        Assert.Throws<ArgumentException>(() => (a + b));
    }

    [Fact]
    public void VectorMinusTestPositive()
    {
        Vector a = new Vector(1, 2);
        Vector b = new Vector(3, 4);
        Assert.Equal(new Vector(-2, -2), a - b);
    }

    [Fact]
    public void VectorMinusTestNegative()
    {
        Vector a = new Vector(1, 0, 0);
        Vector b = new Vector(1, 0);
        Assert.Throws<ArgumentException>(() => a - b);
    }


    [Fact]
    public void VectorEqualityTestPositive()
    {
        Vector a = new Vector(1, 2);
        Vector b = new Vector(1, 2);
        Assert.True(a == b);
    }

    [Fact]
    public void VectorEqualityTestNegative()
    {
        Vector a = new Vector(1, 2);
        Vector b = new Vector(3, 4);
        Assert.False(a == b);
    }

    [Fact]
    public void VectorEqualityTestNegativeDifferentSize()
    {
        Vector a = new Vector(1, 2);
        Vector b = new Vector(3, 4, 5);
        Assert.False(a == b);
    }

    [Fact]
    public void VectorNotEqualityPositive()
    {
        Vector a = new Vector(1, 2);
        Vector b = new Vector(3, 4);
        Assert.True(a != b);
    }

    [Fact]
    public void VectorNotEqualityNegative()
    {
        Vector a = new Vector(1, 2);
        Vector b = new Vector(1, 2);
        Assert.False(a != b);
    }

    [Fact]
    public void VectorEqualTestNegative()
    {
        Vector a = new Vector(5, 5);
        Vector b = new Vector(6, 11, 2);
        Assert.False(a == b);
    }

    [Fact]
    public void VectorEqualTestPositive()
    {
        Vector a = new Vector(5, 5);
        Vector b = new Vector(5, 5);
        Assert.True(a.Equals(b));
        // я умная, проверяю is с другим типом
        Assert.False(a.Equals(new List<int>()));
        // а еще я умная и проверяю с указателем на пустой участок :)
        Assert.False(b.Equals(null));
    }

    [Fact]
    public void VectorGetHashCodeTest()
    {
        Vector a = new Vector(1, 1);
        Vector b = new Vector(1, 1);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void VectorSameSizeTest()
    {
        Vector a = new Vector(5, 5);
        Vector b = new Vector(6, 11);
        Vector c = new Vector(6, 3, 4);
        Assert.True(Vector.IsSameSize(a, b));
        Assert.False(Vector.IsSameSize(a, c));
    }

}

