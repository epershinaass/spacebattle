using System.Collections.Concurrent;
using Moq;

namespace SpaceBattle.Lib.Test;

public class UpdateBehaviourTests
{
    [Fact]
    public void successfulUpdateBehaviour()
    {
        var queue = new BlockingCollection<ICommand>();

        var objToMove = new Mock<IMovable>();
        objToMove.SetupProperty(x => x.Position);
        objToMove.SetupGet(x => x.Velocity).Returns(new Vector(-7, 3));
        objToMove.Object.Position = new Vector(12, 5);
        var cmd = new MoveCommand(objToMove.Object);

        queue.Add(cmd);

        var ra = new ReceiverAdapter(queue);

        var st = new ServerThread(ra);

        var HandleNothing = () => 
        {
            
        };

        new UpdateBehaviourCommand(st, HandleNothing).Execute();

        st.Start();

        Assert.True(objToMove.Object.Position == new Vector(12, 5));
    }
}
