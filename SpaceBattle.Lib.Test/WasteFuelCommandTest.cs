using Moq;
using Xunit;

namespace SpaceBattle.Lib.Test
{
    public class WasteFuelCommandTests
    {
        [Fact]
        public void TestFuelDecreasesCorrectly()
        {
            var mock = new Mock<IFuelChangable>();
            mock.SetupProperty(m => m.fuelLevel, 100);
            mock.SetupGet(m => m.fuelConsumption).Returns(25);

            var cmd = new WasteFuelCommand(mock.Object);

            cmd.Execute();

            Assert.Equal(75, mock.Object.fuelLevel);
        }

        [Fact]
        public void TestThrowsOnGettingConsumption()
        {
            var mock = new Mock<IFuelChangable>();
            mock.SetupGet(m => m.fuelLevel).Returns(100);
            mock.SetupGet(m => m.fuelConsumption).Throws<InvalidOperationException>();

            var cmd = new WasteFuelCommand(mock.Object);

            Assert.Throws<InvalidOperationException>(() => cmd.Execute());
        }

        [Fact]
        public void TestThrowsOnSettingFuelLevel()
        {
            var mock = new Mock<IFuelChangable>();
            mock.SetupGet(m => m.fuelConsumption).Returns(25);
            mock.SetupGet(m => m.fuelLevel).Returns(100);
            mock.SetupSet(m => m.fuelLevel = It.IsAny<float>()).Callback<float>(_ => throw new InvalidOperationException());

            var cmd = new WasteFuelCommand(mock.Object);

            Assert.Throws<InvalidOperationException>(() => cmd.Execute());
        }
    }
}
