using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Test;

public class TestChekCollision
{
    Mock<IStrategy> getDecisionTreeStrategy = new Mock<IStrategy>();
    GetDifferenceStrategy getDifferenceStrategy = new GetDifferenceStrategy();
    GetPropertyStrategy getPropertyStrategy = new GetPropertyStrategy();
    public TestChekCollision()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.GetProperty", (object[] args) => getPropertyStrategy.ExecuteStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.GetDifference", (object[] args) => getDifferenceStrategy.ExecuteStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.GetDecisionTree", (object[] args) => getDecisionTreeStrategy.Object.ExecuteStrategy(args)).Execute();
    }
}
