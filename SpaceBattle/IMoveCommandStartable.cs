namespace SpaceBattle
{
    public interface IMoveCommandStartable
    {

        IUObject Obj { get; }
        Vector InitialVelocity { get; }
    }
}