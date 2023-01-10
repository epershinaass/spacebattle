namespace SpaceBattle;

public class MacroCommand : ICommand
{   
    private List<ICommand> list_cmds;
    public MacroCommand(List<ICommand> list_cmds)
    {
        this.list_cmds = list_cmds;
    }
    public void Execute()
    {
        foreach (var cmd in list_cmds)
        {
            cmd.Execute();
        }
    }
}
