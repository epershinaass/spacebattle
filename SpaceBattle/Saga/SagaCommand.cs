namespace SpaceBattle;

public class SagaCommand : ICommand
{
    List<Tuple<ICommand, ICommand>> cmds;
    int pivotIndex;
    int maxRetries;
    public SagaCommand(List<Tuple<ICommand, ICommand>> _cmds, int _pivotIndex = -1, int _maxRetries = 0)
    {
        cmds = _cmds;
        pivotIndex = _pivotIndex;
        maxRetries = _maxRetries;
    }      
    public void Execute()
    {
        var executedCommands = new Stack<Tuple<ICommand, ICommand>>();
        int i = 0;
        try{     
        for (; i < cmds.Count(); i++) {
                    var command = cmds[i].Item1;
                    if (maxRetries > 0) {
                        command = new RetryCommand(command, maxRetries);
                    }
                    command.Execute();
                    executedCommands.Push(cmds[i]);
        }} catch{

            while (executedCommands.Count > 0) {
                var cmd = executedCommands.Pop();
                if (pivotIndex == -1 || Array.IndexOf(cmds.ToArray(), cmd) <= pivotIndex) {
                    try {
                        var compensation = cmd.Item2;
                        if (maxRetries > 0){
                            compensation = new RetryCommand(compensation, maxRetries);
                        }
                        compensation.Execute();
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            throw;
        }
    }
}
