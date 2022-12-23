namespace SpaceBattle.Lib;

public interface IRotatable
{
    public Angle Angle { get; set; }
    public Angle AngularVelocity { get; }
}
