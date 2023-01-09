using Hwdtech;
namespace SpaceBattle;

public class StrategyGetDifference : IStrategy
{
    public object ExecuteStrategy(params object[] args)
    {
        var obj1 = (IUObject)args[0];
        var obj2 = (IUObject)args[1];
        var properties = new List<string> { "Position", "Velocity" };
        var list = new List<Vector>();
        foreach (string property in properties)
        {
            var obj1_property = IoC.Resolve<Vector>("SpaceBattle.GetProperty", obj1, property);
            var obj2_property = IoC.Resolve<Vector>("SpaceBattle.GetProperty", obj2, property);
            list.Add(obj2_property - obj1_property); 
        }
        return new List<Vector>(list);
    }
}

