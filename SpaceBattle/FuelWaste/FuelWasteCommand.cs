namespace SpaceBattle;
public class WasteFuelCommand : ICommand
{
    IFuelChangable obj;
    public WasteFuelCommand(IFuelChangable _obj)
    {
        obj = _obj;
    }      
    public void Execute()
    {
        var newFuelLevel = obj.fuelLevel - obj.fuelConsumption;

        if (newFuelLevel < 0)
            throw new Exception();

        obj.fuelLevel = newFuelLevel;
    }
}