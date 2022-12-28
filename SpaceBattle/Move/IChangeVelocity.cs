namespace SpaceBattle.Move
{
    // интерфейс, который расширяет интерфейс IMovable
    // позволяет менять скорость
    public interface IChangeVelocity : IMovable
    {
        Vector Velocity { get; set; }
    }
}