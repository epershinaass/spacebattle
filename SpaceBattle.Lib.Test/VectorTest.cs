namespace SpaceBattle.Lib.Test;

    public class VectorTest
    {
       [Fact]
        public void VectorSumTest()
        {
           Vector a = new Vector(5, 5);
           Vector b = new Vector(6, 15);
           Assert.Equal(new Vector(11, 20), a+b);
        }
    }
