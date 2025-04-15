using System.Collections.Concurrent;
using Hwdtech;


namespace SpaceBattle;

public class CreateNewGameConcurrent : IStrategy
{
    int quantum;
    public CreateNewGameConcurrent(int _quantum = 500)
    {
        quantum = _quantum;
    }
    public object ExecuteStrategy(params object[] args)
    {
        ConcurrentQueue<ICommand> queue = new ConcurrentQueue<ICommand>();
        string gameId = IoC.Resolve<string>("Game.MakeNewId");
        ConcurrentDictionary<string, ConcurrentQueue<ICommand>> gamesQueues = IoC.Resolve<ConcurrentDictionary<string, ConcurrentQueue<ICommand>>>("Game.Queue.GetAll");
        gamesQueues[gameId] = queue;
        return IoC.Resolve<ICommand>("Commands.GameCommand", queue);
    }
}