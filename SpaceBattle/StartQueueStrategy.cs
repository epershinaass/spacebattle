namespace SpaceBattle;

public class StartQueueStrategy : IStrategy 
{
    public object ExecuteStrategy(params object[] args)
    {
        var queue = (Queue<ICommand>)args[0];

        return new StartQueue(queue);
    }
}
