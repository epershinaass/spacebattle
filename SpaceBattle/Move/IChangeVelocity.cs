namespace SpaceBattle.Move
{
    public interface IChangeVelocity : IMovable
    {
        Vector Velocity { get; set; }
    }
}
