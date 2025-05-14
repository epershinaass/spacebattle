namespace SpaceBattle.Lib.Test;

using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using SpaceBattle;
using ICommand = SpaceBattle.ICommand;

public class SagaCommandUnitTest
{
    Mock<ICommand> successCommandMock;
    Mock<ICommand> exceptionCommandMock;
    Mock<ICommand> compensatingCommandMock;

    public class AdaptStrategy : IStrategy
    {
        public object ExecuteStrategy(params object[] args)
        {
            return new Mock<object>().Object;
        }
    }

    public SagaCommandUnitTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.SagaCommand",
            (object[] args) => new CreateSaga().ExecuteStrategy(args)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.AdaptForCmd",
            (object[] args) => new AdaptStrategy().ExecuteStrategy(args)).Execute();

        this.successCommandMock = new Mock<ICommand>();
        successCommandMock.Setup(cmd => cmd.Execute()).Verifiable();

        this.exceptionCommandMock = new Mock<ICommand>();
        exceptionCommandMock.Setup(cmd => cmd.Execute()).Throws(new Exception()).Verifiable();

        this.compensatingCommandMock = new Mock<ICommand>();
        compensatingCommandMock.Setup(cmd => cmd.Execute()).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SuccessCommand",
            (object[] args) => successCommandMock.Object).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "AnotherSuccessCommand",
            (object[] args) => successCommandMock.Object).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Undo.AnotherSuccessCommand",
            (object[] args) => successCommandMock.Object).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "RetrySuccessCommand",
            (object[] args) => successCommandMock.Object).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Undo.RetrySuccessCommand",
            (object[] args) => successCommandMock.Object).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionCommand",
            (object[] args) => exceptionCommandMock.Object).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Undo.SuccessCommand",
            (object[] args) => compensatingCommandMock.Object).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Undo.ExceptionCommand",
            (object[] args) => compensatingCommandMock.Object).Execute();
    }

    [Fact]
    public void SagaExecutesSuccessfully_NoRollback()
    {
        var obj = new Mock<IUObject>();
        var saga = IoC.Resolve<ICommand>("Game.Commands.SagaCommand",
            "SuccessCommand", "AnotherSuccessCommand", "AnotherSuccessCommand", obj.Object, 0);

        saga.Execute();

        successCommandMock.Verify(cmd => cmd.Execute(), Times.Exactly(2));
        exceptionCommandMock.Verify(cmd => cmd.Execute(), Times.Never);
        compensatingCommandMock.Verify(cmd => cmd.Execute(), Times.Never);
    }

    [Fact]
    public void SagaFailsAndRollsBack()
    {
        var obj = new Mock<IUObject>();
        var saga = IoC.Resolve<ICommand>("Game.Commands.SagaCommand",
            "SuccessCommand", "ExceptionCommand", "ExceptionCommand", obj.Object, 0);

        Assert.Throws<Exception>(() => saga.Execute());

        successCommandMock.Verify(cmd => cmd.Execute(), Times.Once);
        exceptionCommandMock.Verify(cmd => cmd.Execute(), Times.Once);
        compensatingCommandMock.Verify(cmd => cmd.Execute(), Times.Once);
    }

    [Fact]
    public void SagaWithThreeSuccessfulCommands()
    {
        var obj = new Mock<IUObject>();
        var saga = IoC.Resolve<ICommand>("Game.Commands.SagaCommand",
            "SuccessCommand", "AnotherSuccessCommand", "RetrySuccessCommand", "RetrySuccessCommand", obj.Object, 0);

        saga.Execute();

        successCommandMock.Verify(cmd => cmd.Execute(), Times.Exactly(3));
        exceptionCommandMock.Verify(cmd => cmd.Execute(), Times.Never);
        compensatingCommandMock.Verify(cmd => cmd.Execute(), Times.Never);
    }
}
