namespace SpaceBattle;

public interface IStrategy
{
    public object ExecuteStrategy(params object[] args);
}