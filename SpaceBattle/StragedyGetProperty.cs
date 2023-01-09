using SpaceBattle;

public class StrategyGetProperty : IStrategy
{
    public object ExecuteStrategy(params object[] args)
    {
        var obj = (IUObject)args[0];
        var property = (string)args[1];
        return obj.GetProperty(property);
    }
}
