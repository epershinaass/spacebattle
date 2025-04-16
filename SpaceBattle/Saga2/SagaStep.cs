namespace SpaceBattle;

public class SagaStep : ISagaStep
{
    private readonly ICommand _action;
    private readonly ICommand _compensation;

    public SagaStep(ICommand action, ICommand compensation)
    {
        _action = action;
        _compensation = compensation;
    }

    public void Execute()
    {
        _action.Execute();
    }

    public void Compensate()
    {
        _compensation.Execute();
    }
}
