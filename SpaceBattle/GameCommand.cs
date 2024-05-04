using Hwdtech;


namespace SpaceBattle;


public class GameCommand : SpaceBattle.ICommand
{
    private readonly Queue<SpaceBattle.ICommand> queue;

    private readonly object scope;

    public GameCommand(object scope, Queue<SpaceBattle.ICommand> queue)
    {
        this.scope = scope;
        this.queue = queue;
    }

    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        IoC.Resolve<SpaceBattle.ICommand>("StartQueue", queue).Execute();
    }
}
