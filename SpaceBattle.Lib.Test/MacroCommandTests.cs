using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Test;

public class TestMacroCommand
{
    public TestMacroCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var cmd = new Mock<ICommand>();
        cmd.Setup(c => c.Execute());

        var strategy = new Mock<IStrategy>();
        strategy.Setup(c => c.ExecuteStrategy(It.IsAny<object[]>())).Returns(cmd.Object);
        
        var list = new Mock<IStrategy>();
        list.Setup(c => c.ExecuteStrategy()).Returns(new List<string> { "Command" });

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.Operation.Move", (object[] args) => list.Object.ExecuteStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command", (object[] args) => strategy.Object.ExecuteStrategy(args)).Execute();
    }

    [Fact]
    public void PositiveMacroCommandTest()
    {
        var obj = new Mock<IUObject>();
        var createMacroCommand = new MacroCommandStrategy();
        var macroCommand = (ICommand)createMacroCommand.ExecuteStrategy(obj.Object, "Move");
        macroCommand.Execute();
    }
}
