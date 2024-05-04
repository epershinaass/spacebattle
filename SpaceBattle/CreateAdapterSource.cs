namespace SpaceBattle;
using System.Reflection;

public class CreateAdapterSource: IStrategy
{
    Type target;
    public CreateAdapterSource(Type targetType)
    {
        target = targetType;
    }
    public object ExecuteStrategy(object[] args)
    {
        string header = $"namespace SpaceBattle;public class {target.Name}Adapter : {target.Name}";
        string openFigBracket = "{";
        string closingFigBracket = "}";

        string targetField = $"IUObject target;";
        string constructor = $"public {target.Name}Adapter(object _target)" + openFigBracket + $"target = (IUObject) _target;" + closingFigBracket;
        
        PropertyInfo[] properties = target.GetProperties();

        string fields = "";
        
        foreach (PropertyInfo prop in properties)
        {
            string field = $"public {prop.PropertyType} {prop.Name}" + openFigBracket;
            if (prop.GetMethod != null)
            {
                field += $"get => ({prop.PropertyType}) target.GetProperty(\"{prop.Name}\");";
            }
            if (prop.SetMethod != null)
            {
                field += $"set => target.SetProperty(\"{prop.Name}\", value);";
            }
            field += closingFigBracket;
            fields += field;
        }

        string result = header + openFigBracket + targetField + constructor + fields + closingFigBracket;
        return result;
    }
}
