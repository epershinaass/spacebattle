namespace SpaceBattle.Lib.Test;

    public class VectorTest
    {
       [Fact]
        public void VectorSumTestPositive()
        {
           Vector a = new Vector(5, 5);
           Vector b = new Vector(6, 15);
           Assert.Equal(new Vector(11, 20), a+b);
        }

        [Fact]
        public void VectorSumTestNegative()
        {
           Vector a = new Vector(5, 5);
           Vector b = new Vector(6, 15, 2);
           Assert.Throws<ArgumentException>(() => (a+b));
        }

        [Fact]
        public void VectorToStringTestPositive()
        {
           Vector a = new Vector(5, 5);
           Assert.Equal("(5, 5)", a.ToString());
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
        }

         [Fact]
      public void VectorGetHashCodeTest()
      {
        Vector a = new Vector(1, 2);
        Vector b = new Vector(1, 2);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
      }

      }
    
