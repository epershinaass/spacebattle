using Hwdtech;


namespace SpaceBattle;


public class GameCommand : ICommand
{
    private Queue<ICommand> queue;

    private object scope;

    public GameCommand(object scope, Queue<ICommand> queue)
    {
        this.scope = scope;
        this.queue = queue;
    }

    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        IoC.Resolve<ICommand>("StartQueue", queue).Execute();
    }
}
