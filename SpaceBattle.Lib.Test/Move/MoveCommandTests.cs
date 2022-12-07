namespace SpaceBattle.Lib.Test;

    public class MoveCommandTests
    {
       [Fact]
        public void TestPositiveMove()
        {
            Mock<IMovable> movable = new Mock<IMovable>();
            movable.SetupProperty(m => m.Position, new Vector(0, 0));
            movable.SetupGet<Vector>(m => m.Velocity).Returns(new Vector(1, 1));
            MoveCommand mc = new MoveCommand(movable.Object);
            mc.Execute();
            Assert.Equal(new Vector(1, 1), movable.Object.Position);
        }

        [Fact]
        public void TestGetVelocity()
        {
            Mock<IMovable> movable = new Mock<IMovable>();
            movable.SetupProperty(m => m.Position, new Vector(0, 0));
            movable.SetupGet<Vector>(m => m.Velocity).Throws<ArgumentException>();
            MoveCommand mc = new MoveCommand(movable.Object);
            Assert.Throws<ArgumentException>(() => mc.Execute());
        }

        [Fact]
        public void TestGetPosition()
        {
            Mock<IMovable> movable = new Mock<IMovable>();
            movable.SetupProperty(m => m.Position, new Vector(0, 0));
            movable.SetupGet<Vector>(m => m.Velocity).Returns(new Vector(1, 1));
            movable.SetupGet<Vector>(m => m.Velocity).Throws<ArgumentException>();
            MoveCommand mc = new MoveCommand(movable.Object);
            Assert.Throws<ArgumentException>(() => mc.Execute());
        }

        [Fact]
        public void TestSetPosition()
        {
            Mock<IMovable> movable = new Mock<IMovable>();
            movable.SetupProperty(m => m.Position, new Vector(0, 0));
            movable.SetupGet<Vector>(m => m.Velocity).Returns(new Vector(1, 1));
            movable.SetupSet<Vector>(m => m.Position = It.IsAny<Vector>()).Throws<ArgumentException>();
            MoveCommand mc = new MoveCommand(movable.Object);
            Assert.Throws<ArgumentException>(() => mc.Execute());
        }
    }
