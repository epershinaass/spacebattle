namespace SpaceBattle.Lib.Test;
using Moq;

public class RotateTest
{
    [Fact]
    public void SuccessfullRotateCommand()
    {
        Mock<IRotatable> rotatableMock = new Mock<IRotatable>();
        rotatableMock.SetupProperty<Angle>(m => m.Angle, new Angle(45, 1));
        rotatableMock.SetupGet<Angle>(m => m.AngularVelocity).Returns(new Angle(90, 1));


        ICommand rotateCommand = new RotateCommand(rotatableMock.Object);
        rotateCommand.Execute();
        Assert.Equal(new Angle(135, 1), rotatableMock.Object.Angle);
    }

    [Fact]
    public void AngleNotSetException()
    {
        Mock<IRotatable> rotatableMock = new Mock<IRotatable>();
        rotatableMock.SetupProperty(m => m.Angle, new Angle(45, 1));
        rotatableMock.SetupGet<Angle>(m => m.AngularVelocity).Returns(new Angle(90, 1));
        rotatableMock.SetupSet<Angle>(m => m.Angle = It.IsAny<Angle>()).Throws<Exception>();
        ICommand rotateCommand = new RotateCommand(rotatableMock.Object);

        Assert.Throws<Exception>(() => rotateCommand.Execute());


    }

    [Fact]
    public void AngleNotGetException()
    {
        Mock<IRotatable> rotatableMock = new Mock<IRotatable>();

        rotatableMock.SetupGet<Angle>(m => m.AngularVelocity).Returns(new Angle(45, 1));
        rotatableMock.SetupSet<Angle>(m => m.Angle = new Angle(90, 1));
        rotatableMock.SetupGet<Angle>(m => m.Angle).Throws<Exception>();
        ICommand rotateCommand = new RotateCommand(rotatableMock.Object);
        Assert.Throws<Exception>(() => rotateCommand.Execute());

    }

    [Fact]
    public void AnglularVelocityNotGetException()
    {
        Mock<IRotatable> rotatableMock = new Mock<IRotatable>();
        rotatableMock.SetupProperty<Angle>(m => m.Angle, new Angle(90, 1));
        rotatableMock.SetupGet<Angle>(m => m.AngularVelocity).Throws<Exception>();
        ICommand rotateCommand = new RotateCommand(rotatableMock.Object);
        Assert.Throws<Exception>(() => rotateCommand.Execute());
    }
}