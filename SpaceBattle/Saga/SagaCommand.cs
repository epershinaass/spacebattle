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
        int i = 0;
        try{     
        for (; i < cmds.Count(); i++) {
                    var command = cmds[i].Item1;
                    if (maxRetries > 0) {
                        command = new RetryCommand(command, maxRetries);
                    }
                    command.Execute();
        }} catch{
            i -= 1;
            for (; i >= 0; i--){
                if (pivotIndex == -1 || i <= pivotIndex){
                    try{
                        cmds[i].Item2.Execute();
                    }
                    catch{
                        continue;
                    }
                    
        }
    }
    throw;
}
}
}
