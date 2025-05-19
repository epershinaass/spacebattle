using System.Collections.Concurrent;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;
using System.Security.Cryptography;

namespace SpaceBattle.Lib.Test;

public class ShardedThreadsTests
{
    int numberOfGames;
    public ShardedThreadsTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        numberOfGames = 1;

        ConcurrentDictionary<string, ConcurrentQueue<ICommand>> gamesQueues = new ConcurrentDictionary<string, ConcurrentQueue<ICommand>>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.GetAll", (object[] args) => gamesQueues).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.Get", (object[] args) => gamesQueues[(string)args[0]]).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.MakeNewId", (object[] args) => "game" + numberOfGames++.ToString()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GameCommand", (object[] args) => new Mock<ICommand>().Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.InterpretMessage", (object[] args) => new InterpretThreadMessageCommand((IMessage)args[0])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateCommand", (object[] args) => new Mock<ICommand>().Object).Execute();
    }

    [Fact]
    public void GameCreationTest()
    {
        numberOfGames = 1;
        BlockingCollection<ICommand> gameQueue1 = new BlockingCollection<ICommand>();
        BlockingCollection<ICommand> gameQueue2 = new BlockingCollection<ICommand>();

        BlockingCollection<ICommand> msgQueue1 = new BlockingCollection<ICommand>();
        BlockingCollection<ICommand> msgQueue2 = new BlockingCollection<ICommand>();

        ThreadMessageSenderAdapter msa1 = new ThreadMessageSenderAdapter(msgQueue1);
        ThreadMessageSenderAdapter msa2 = new ThreadMessageSenderAdapter(msgQueue2);
        Dictionary<string, ThreadMessageSenderAdapter> msgSenders = new Dictionary<string, ThreadMessageSenderAdapter>();
        msgSenders["th1"] = msa1;
        msgSenders["th2"] = msa2;

        ReceiverAdapter gra1 = new ReceiverAdapter(gameQueue1);
        ReceiverAdapter gra2 = new ReceiverAdapter(gameQueue2);

        ReceiverAdapter mra1 = new ReceiverAdapter(msgQueue1);
        ReceiverAdapter mra2 = new ReceiverAdapter(msgQueue2);

        ServerThread thread1 = new ServerThread(gra1, mra1);
        ServerThread thread2 = new ServerThread(gra2, mra2);

        List<ServerThread> threads = new List<ServerThread> { thread1, thread2 };

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Threading.Thread.Get", (object[] args) => threads[(int)args[0]]).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Threading.Get.MessageSender", (object[] args) => msgSenders[(string)args[0]]).Execute();

        ConcurrentDictionary<string, List<string>> threadsGames = new ConcurrentDictionary<string, List<string>>();
        threadsGames["th1"] = new List<string>();
        threadsGames["th2"] = new List<string>();

        new CreateNewGameConcurrent().ExecuteStrategy();

        var allGames = IoC.Resolve<ConcurrentDictionary<string, ConcurrentQueue<ICommand>>>("Game.Queue.GetAll");
        Assert.Single(allGames);

        new CreateNewGameConcurrent().ExecuteStrategy();

        Assert.Equal(2, IoC.Resolve<ConcurrentDictionary<string, ConcurrentQueue<ICommand>>>("Game.Queue.GetAll").Count);

    }
    [Fact]
    public void MessageSendingTest()
    {
        numberOfGames = 1;
        BlockingCollection<ICommand> gameQueue1 = new BlockingCollection<ICommand>();

        BlockingCollection<ICommand> msgQueue1 = new BlockingCollection<ICommand>();

        ThreadMessageSenderAdapter msa1 = new ThreadMessageSenderAdapter(msgQueue1);

        Dictionary<string, ThreadMessageSenderAdapter> msgSenders = new Dictionary<string, ThreadMessageSenderAdapter>();
        msgSenders["th1"] = msa1;

        ReceiverAdapter gra1 = new ReceiverAdapter(gameQueue1);

        ReceiverAdapter mra1 = new ReceiverAdapter(msgQueue1);

        ServerThread thread1 = new ServerThread(gra1, mra1);

        List<ServerThread> threads = new List<ServerThread> { thread1 };

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Threading.Thread.Get", (object[] args) => threads[(int)args[0]]).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Threading.Get.MessageSender", (object[] args) => msgSenders[(string)args[0]]).Execute();

        ConcurrentDictionary<string, List<string>> threadsGames = new ConcurrentDictionary<string, List<string>>();
        threadsGames["th1"] = new List<string>();

        new CreateNewGameConcurrent().ExecuteStrategy();

        threadsGames["th1"].Add("game1");

        Mock<IMessage> mockMsg = new Mock<IMessage>();
        mockMsg.Setup(x => x.Gameid).Returns("game1");

        ShardedThreadsMessagesInterpreter shThreads = new ShardedThreadsMessagesInterpreter(threadsGames);

        shThreads.sendMessage(mockMsg.Object);

        Assert.NotEmpty(msgQueue1);
    }
    [Fact]
    public void MessageInterpretationTest()
    {
        numberOfGames = 1;
        BlockingCollection<ICommand> gameQueue1 = new BlockingCollection<ICommand>();

        BlockingCollection<ICommand> msgQueue1 = new BlockingCollection<ICommand>();

        ThreadMessageSenderAdapter msa1 = new ThreadMessageSenderAdapter(msgQueue1);

        Dictionary<string, ThreadMessageSenderAdapter> msgSenders = new Dictionary<string, ThreadMessageSenderAdapter>();
        msgSenders["th1"] = msa1;

        ReceiverAdapter gra1 = new ReceiverAdapter(gameQueue1);

        ReceiverAdapter mra1 = new ReceiverAdapter(msgQueue1);

        ServerThread thread1 = new ServerThread(gra1, mra1);

        List<ServerThread> threads = new List<ServerThread> { thread1 };

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Threading.Thread.Get", (object[] args) => threads[(int)args[0]]).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Threading.Get.MessageSender", (object[] args) => msgSenders[(string)args[0]]).Execute();

        ConcurrentDictionary<string, List<string>> threadsGames = new ConcurrentDictionary<string, List<string>>();
        threadsGames["th1"] = new List<string>();

        new CreateNewGameConcurrent().ExecuteStrategy();

        threadsGames["th1"].Add("game1");

        Mock<IMessage> mockMsg = new Mock<IMessage>();
        mockMsg.Setup(x => x.Gameid).Returns("game1");

        ShardedThreadsMessagesInterpreter shThreads = new ShardedThreadsMessagesInterpreter(threadsGames);

        shThreads.sendMessage(mockMsg.Object);

        msgQueue1.Take().Execute();

        var gameQueue = IoC.Resolve<ConcurrentDictionary<string, ConcurrentQueue<ICommand>>>("Game.Queue.GetAll")["game1"];
        Assert.NotEmpty(gameQueue);
    }
}
