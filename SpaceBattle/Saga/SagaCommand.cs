namespace SpaceBattle;

public class SagaCommand : ICommand
{
    private List<Tuple<ICommand, ICommand>> beforePivot;
    private ICommand pivotCommand;
    private List<Tuple<ICommand, ICommand>> afterPivot;
    private int maxRetries;

    public SagaCommand(
        List<Tuple<ICommand, ICommand>> beforePivot,
        ICommand pivotCommand,
        List<Tuple<ICommand, ICommand>> afterPivot,
        int maxRetries = 0)
    {
        this.beforePivot = beforePivot;
        this.pivotCommand = pivotCommand;
        this.afterPivot = afterPivot;
        this.maxRetries = maxRetries;
    }
    public void Execute()
    {
        int attempt = 0;
        while (true)
        {
            var executedBefore = new Stack<ICommand>();
            var executedAfter = new Stack<ICommand>();
            try
            {
                for (int i = 0; i < beforePivot.Count; i++)
                {
                    beforePivot[i].Item1.Execute();
                    executedBefore.Push(beforePivot[i].Item1);
                }
                pivotCommand.Execute();

                for (int i = 0; i < afterPivot.Count; i++)
                {
                    afterPivot[i].Item1.Execute();
                    executedAfter.Push(afterPivot[i].Item1);
                }
                break;

            }
            catch
            {
                Rollback(executedBefore, executedAfter);
                attempt++;
                if (attempt > maxRetries)
                {
                    throw;
                }
            }
        }
    }

    public void Rollback(Stack<ICommand> executedBefore, Stack<ICommand> executedAfter)
    {
        for (int i = beforePivot.Count - 1; i >= 0; i--)
        {
            var undoCmd = beforePivot[i].Item2;
            try
            {
                undoCmd.Execute();
            }
            catch { }
            while (executedAfter.Count > 0)
            {
                var cmd = executedAfter.Pop();
                int idx = afterPivot.FindIndex(t => t.Item1 == cmd);
                if (idx >= 0)
                {
                    try
                    {
                        afterPivot[idx].Item2.Execute();
                    }
                    catch { }
                }
            }
        }
    }
}
