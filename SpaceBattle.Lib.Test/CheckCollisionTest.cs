using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Test;

public class TestChekCollision
{
    Mock<IStrategy> getDecisionTreeStrategy = new Mock<IStrategy>();
    StrategyGetDifference getDifferenceStrategy = new StrategyGetDifference();
    StrategyGetProperty getPropertyStrategy = new StrategyGetProperty();
    public TestChekCollision()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.GetProperty", (object[] args) => getPropertyStrategy.ExecuteStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.GetDifference", (object[] args) => getDifferenceStrategy.ExecuteStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.GetDecisionTree", (object[] args) => getDecisionTreeStrategy.Object.ExecuteStrategy(args)).Execute();
    }

    [Fact]
    public void CollisionsFoundTest()
    {
        var obj1 = new Mock<IUObject>();
        var obj2 = new Mock<IUObject>();

        obj1.Setup(x => x.GetProperty("Velocity")).Returns(new Vector(It.IsAny<int>(), It.IsAny<int>()));
        obj1.Setup(x => x.GetProperty("Position")).Returns(new Vector(It.IsAny<int>(), It.IsAny<int>()));
        obj2.Setup(x => x.GetProperty("Velocity")).Returns(new Vector(It.IsAny<int>(), It.IsAny<int>()));
        obj2.Setup(x => x.GetProperty("Position")).Returns(new Vector(It.IsAny<int>(), It.IsAny<int>()));

        getDecisionTreeStrategy.Setup(c => c.ExecuteStrategy(It.IsAny<object[]>())).Returns(true).Verifiable();
        CheckCollision collision = new CheckCollision(obj1.Object, obj2.Object);

        Assert.Throws<Exception>(() => collision.Execute());
        getDecisionTreeStrategy.Verify();
    }

}