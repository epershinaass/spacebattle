using System.Collections.Concurrent;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;


public class TestObject : IUObject
{
    Dictionary<string, object> properties;
    public TestObject(Dictionary<string, object> props)
    {
        properties = props;
    }
    public object GetProperty(string key)
    {
        return properties[key];
    }

    public void SetProperty(string key, object value)
    {
        properties[key] = value;
    }
}

public class AdapterTests
{
    [Fact]
    public void StringSourceTest()
    {
        string header = "namespace SpaceBattle;public class IMovableAdapter : IMovable";

        string openFigBracket = "{";
        string closingFigBracket = "}";

        string targetField = "IUObject target;";
        string constructor = "public IMovableAdapter(object _target){target = (IUObject) _target;}";

        string posHeader = "public SpaceBattle.Vector Position";
        string posGet = "get => (SpaceBattle.Vector) target.GetProperty(\"Position\");";
        string posSet = "set => target.SetProperty(\"Position\", value);";

        string velocityHeader = "public SpaceBattle.Vector Velocity";
        string velocityGet = "get => (SpaceBattle.Vector) target.GetProperty(\"Velocity\");";

        string expected = header + openFigBracket + targetField + constructor + posHeader + openFigBracket + posGet + posSet + closingFigBracket + velocityHeader + openFigBracket + velocityGet + closingFigBracket + closingFigBracket;

        Type? IMovableType = Type.GetType("SpaceBattle.IMovable, SpaceBattle", true, true);

        string adapterSource = (string) new CreateAdapterSource(IMovableType!).ExecuteStrategy(new object[] {});

        Assert.Equal(expected, adapterSource);
    }
}
