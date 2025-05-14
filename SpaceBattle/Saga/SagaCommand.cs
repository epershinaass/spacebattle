namespace SpaceBattle;

public class SagaCommand : ICommand
{
    private List<Tuple<ICommand, ICommand>> beforePivot;
    private ICommand pivotCommand;
    private List<ICommand> afterPivot;
    private int maxRetries;

    public SagaCommand(
        List<Tuple<ICommand, ICommand>> beforePivot,
        ICommand pivotCommand,
        List<ICommand> afterPivot,
        int maxRetries = 0) {
            this.beforePivot = beforePivot;
            this.pivotCommand = pivotCommand;
            this.afterPivot = afterPivot;
            this.maxRetries = maxRetries;
        }      
    public void Execute()
    {
        var executed = new Stack<ICommand>();
        int i = 0;
        try{     
        for(; i < beforePivot.Count; i++) 
        {
            var cmd = WrapRetry(beforePivot[i].Item1);
            cmd.Execute();
            executed.Push(beforePivot[i].Item1); 
        }
            WrapRetry(pivotCommand).Execute();
        } catch {
            Rollback(executed);
            throw;
        }
        foreach (var cmd in afterPivot) {
            WrapRetry(cmd).Execute();
        }
    }
    
    public void Rollback(Stack<ICommand> executed) {
        for (int i = beforePivot.Count - 1; i>= 0; i--) {
            var cmd = beforePivot[i];
            if (executed.Contains(cmd.Item1)) {
                try {
                    WrapRetry(cmd.Item2).Execute();
                } catch {
                }
            }
        }
    }
    private ICommand WrapRetry(ICommand cmd) => 
    maxRetries > 0 ? new RetryCommand(cmd, maxRetries) : cmd;
}

    
