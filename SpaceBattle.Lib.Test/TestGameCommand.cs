namespace SpaceBattle.Lib.Test;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;


public class TestGameCommand
{
    public TestGameCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }



    [Fact]
    public void QueueDequeueException()
    {
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));


        IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();
        var mockStrategy = new Mock<IStrategy>();
        mockStrategy.Setup(x => x.ExecuteStrategy()).Returns(1400);

        IoC.Resolve<ICommand>("IoC.Register", "StartQueue", (object[] args) => new StartQueueStrategy().ExecuteStrategy(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "QueueDequeue", (object[] args) => new QueueDequeue().ExecuteStrategy(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "GetQuantum", (object[] args) => mockStrategy.Object.ExecuteStrategy(args)).Execute();
        Queue<SpaceBattle.ICommand> queue = new Queue<SpaceBattle.ICommand>();

        var scopeNew = IoC.Resolve<object>("Scopes.New", scope);

        var gameCmd = new GameCommand(scopeNew, queue);
        Assert.Throws<Exception>(
            () =>
            {
                gameCmd.Execute();
            }
        );
    }

    [Fact]
    public void TestHandleException()
    {
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));

        var mockHandler = new Mock<IStrategy>();
        mockHandler.Setup(x => x.ExecuteStrategy(It.IsAny<object[]>())).Verifiable();

        IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();

        var mockStrategy = new Mock<IStrategy>();
        mockStrategy.Setup(x => x.ExecuteStrategy()).Returns(1400);

        IoC.Resolve<ICommand>("IoC.Register", "StartQueue", (object[] args) => new StartQueueStrategy().ExecuteStrategy(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "QueueDequeue", (object[] args) => new QueueDequeue().ExecuteStrategy(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "ExceptionHandler", (object[] args) => mockHandler.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "GetQuantum", (object[] args) => mockStrategy.Object.ExecuteStrategy(args)).Execute();

        Queue<SpaceBattle.ICommand> queue = new Queue<SpaceBattle.ICommand>();
        queue.Enqueue(new ActionCommand(() => { }));
        queue.Enqueue(new ActionCommand(() => { throw new Exception(); }));
        queue.Enqueue(new ActionCommand(() => { mockStrategy.Setup(x => x.ExecuteStrategy()).Returns(0); }));

        var scopeNew = IoC.Resolve<object>("Scopes.New", scope);

        var gameCmd = new GameCommand(scopeNew, queue);

        gameCmd.Execute();

        mockHandler.Verify(x => x.ExecuteStrategy(It.IsAny<object[]>()), Times.Once);
        Assert.True(queue.Count == 0);
    }

    [Fact]
    public void TestDefaultHandler()
    {
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));

        IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();

        var mockStrategy = new Mock<IStrategy>();
        mockStrategy.Setup(x => x.ExecuteStrategy()).Returns(1400);

        IoC.Resolve<ICommand>("IoC.Register", "StartQueue", (object[] args) => new StartQueueStrategy().ExecuteStrategy(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "QueueDequeue", (object[] args) => new QueueDequeue().ExecuteStrategy(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "ExceptionHandler", (object[] args) => new DefaultHandler()).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "GetQuantum", (object[] args) => mockStrategy.Object.ExecuteStrategy(args)).Execute();

        Queue<SpaceBattle.ICommand> queue = new Queue<SpaceBattle.ICommand>();
        queue.Enqueue(new ActionCommand(() => { }));
        queue.Enqueue(new ActionCommand(() => { throw new Exception(); }));
        queue.Enqueue(new ActionCommand(() => { mockStrategy.Setup(x => x.ExecuteStrategy()).Returns(0); }));

        var scopeNew = IoC.Resolve<object>("Scopes.New", scope);

        var gameCmd = new GameCommand(scopeNew, queue);

        Assert.Throws<Exception>(
            () =>
            {
                gameCmd.Execute();
            }
        );
    }

    [Fact]
    public void TestSuccessfull()
    {
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));

        IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();

        var mockStrategy = new Mock<IStrategy>();
        mockStrategy.Setup(x => x.ExecuteStrategy()).Returns(140000);

        IoC.Resolve<ICommand>("IoC.Register", "StartQueue", (object[] args) => new StartQueueStrategy().ExecuteStrategy(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "QueueDequeue", (object[] args) => new QueueDequeue().ExecuteStrategy(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "GetQuantum", (object[] args) => mockStrategy.Object.ExecuteStrategy(args)).Execute();

        Queue<SpaceBattle.ICommand> queue = new Queue<SpaceBattle.ICommand>();
        queue.Enqueue(new ActionCommand(() => { mockStrategy.Setup(x => x.ExecuteStrategy()).Returns(-1); }));

        var scopeNew = IoC.Resolve<object>("Scopes.New", scope);

        var gameCmd = new GameCommand(scopeNew, queue);

        gameCmd.Execute();
        Assert.True(queue.Count == 0);
        Assert.Equal(scopeNew, IoC.Resolve<object>("Scopes.Current"));
    }
}
