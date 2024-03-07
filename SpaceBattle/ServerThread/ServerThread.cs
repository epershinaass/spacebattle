namespace SpaceBattle.Lib;


public class ServerThread
{
    public Thread thread { get; private set; }
    public ReceiverAdapter queue { get; private set; }
    bool stop = false;
    Action strategy;
    Action finishingStrategy;
    public ServerThread(ReceiverAdapter queue)
    {
        this.queue = queue;
        strategy = () =>
        {
            handleCommand();
        };

        finishingStrategy = new Action(() =>
        {
        });

        thread = new Thread(() =>
        {
            while (!stop)
            {
                strategy();
            }
        });
    }
    internal void _stop()
    {
        finishingStrategy();
        stop = true;
    }
    internal void handleCommand()
    {
        queue.Receive().Execute();
    }
    internal void updateBehaviour(Action newBehaviour)
    {
        strategy = newBehaviour;
    }
    internal void updateFinishingBehaviour(Action newBehaviour)
    {
        finishingStrategy = newBehaviour;
    }
    public void Start()
    {
        thread.Start();
    }
}