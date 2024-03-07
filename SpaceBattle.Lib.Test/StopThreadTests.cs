using System.Collections.Concurrent;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class StopThreadTests
{
    public StopThreadTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.ActionCommand", (object[] args) => new ActionCommand((Action)args[0])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.InterpretMessage", (object[] args) => (ICommand)args[0]).Execute();

        var serverThreads = new Dictionary<int, (ServerThread, SenderAdapter)>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Threading.ServerThreads", (object[] args) => serverThreads).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Threading.CreateAndStartThread", (object[] args) => new ActionCommand(new Action(
            () =>
            {
                var queue = new BlockingCollection<ICommand>();
                
                if (args.Length == 2)
                {
                    queue.Add(IoC.Resolve<ICommand>("Commands.ActionCommand", (Action)args[1]));
                }
                var receiverQueue = new ReceiverAdapter(queue);
                var senderQueue = new SenderAdapter(queue);
                IoC.Resolve<Dictionary<int, (ServerThread, SenderAdapter)>>("Threading.ServerThreads")
                .Add((int)args[0], (new ServerThread(receiverQueue), senderQueue));
                IoC.Resolve<Dictionary<int, (ServerThread, SenderAdapter)>>("Threading.ServerThreads")[(int)args[0]].Item1.Start();
            }
        ))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Threading.SendCommand", (object[] args) => new ActionCommand(new Action(
            () =>
            {
                int threadId = (int)args[0];
                ICommand cmd = (ICommand)args[1];
                var threads = IoC.Resolve<Dictionary<int, (ServerThread, SenderAdapter)>>("Threading.ServerThreads");
                threads[threadId].Item2.Send((object)cmd);
            }
        ))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Threading.GetThreadId", (object[] args) =>
        {
            var thread = (ServerThread)args[0];
            return (object) IoC.Resolve<Dictionary<int, (ServerThread, SenderAdapter)>>("Threading.ServerThreads")
            .Where(x => x.Value.Item1 == thread).ToList()[0].Key;
        }
        ).Execute();
    }

    [Fact]
    public void successfulStop()
    {
        var objToMove = new Mock<IMovable>();
        objToMove.SetupProperty(x => x.Position);
        objToMove.SetupGet(x => x.Velocity).Returns(new Vector(-7, 3));
        objToMove.Object.Position = new Vector(12, 5);
        var cmd = new MoveCommand(objToMove.Object);

        IoC.Resolve<ICommand>("Threading.CreateAndStartThread", 1).Execute();
        new StopThreadCommand(IoC.Resolve<Dictionary<int, (ServerThread, SenderAdapter)>>("Threading.ServerThreads")[1].Item1).Execute();

        IoC.Resolve<ICommand>("Threading.SendCommand", 1, cmd).Execute();

        Assert.False(objToMove.Object.Position == new Vector(5, 8));
    }
}