using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Test;

public class StartMoveCommandTest 
{
    public TestStartMoveCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",  IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var cmd = new Mock<ICommand>();
        cmd.Setup(c => c.Execute());
        
        var returnCmd = new Mock<IStrategy>();
        returnCmd.Setup(c => c.ExecuteStrategy(It.IsAny<object[]>())).Returns(cmd.Object);

        var returnQueue = new Mock<IStrategy>();
        returnQueue.Setup(x => x.ExecuteStrategy()).Returns(new Queue<ICommand>());

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.SetupProperty", (object[] args) => returnCmd.Object.ExecuteStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.SetupCommand", (object[] args) => returnCmd.Object.ExecuteStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.Move", (object[] args) => returnCmd.Object.ExecuteStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.Queue", (object[] args) => returnQueue.Object.ExecuteStrategy()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.QueuePush", (object[] args) => returnCmd.Object.ExecuteStrategy(args)).Execute();

    }
        
    [Fact]
    public void PositiveTestStartMoveCommand()
    {
        var move_startable = new Mock<IMoveStartable>();
        move_startable.SetupGet(c => c.Obj).Returns(new Mock<IUObject>().Object).Verifiable();
        move_startable.SetupGet(c => c.InitialVelocity).Returns(new Vector(It.IsAny<int>(), It.IsAny<int>())).Verifiable();
            
        ICommand startMove = new StartMoveCommand(move_startable.Object);
        startMove.Execute();
        move_startable.Verify();
    }

        [Fact]
        public void TestImpossibleGetObject()
        {
            var move_startable = new Mock<IMoveStartable>();
            move_startable.SetupGet(c => c.Obj).Throws<Exception>().Verifiable();
            move_startable.SetupGet(c => c.InitialVelocity).Returns(new Vector(It.IsAny<int>(), It.IsAny<int>())).Verifiable();

            ICommand startMove = new StartMoveCommand(move_startable.Object);

            Assert.Throws<Exception>(() => startMove.Execute());
        }

        [Fact]
        public void TestImpossibleGetVelocity()
        {
            var move_startable = new Mock<IMoveStartable>();
            move_startable.SetupGet(a => a.Obj).Returns(new Mock<IUObject>().Object).Verifiable();
            move_startable.SetupGet(a => a.InitialVelocity).Throws<Exception>().Verifiable();

            ICommand startMove = new StartMoveCommand(move_startable.Object);

            Assert.Throws<Exception>(() => startMove.Execute());
        }
            
}