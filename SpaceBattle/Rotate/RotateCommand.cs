namespace SpaceBattle.Lib;

public class RotateCommand: ICommand
{
    private IRotatable rotatable;
    public RotateCommand(IRotatable rotatable)
    {
        this.rotatable = rotatable;
    }

    public void Execute()
    {
        rotatable.Angle += rotatable.AngleVelocity;
    }
}
