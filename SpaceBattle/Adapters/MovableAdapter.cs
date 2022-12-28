using SpaceBattle;
using SpaceBattle.Move;

namespace SpaceBattle.Adapters
{
    // класс, который используется для преобразования IUObject в IChangeVelocity
    public class MovableAdapter : IChangeVelocity
    {
        // тот самый IUObject, который мы хотим преобразовать
        private IUObject instance;

        public MovableAdapter(IUObject instance)
        {
            // получаем его значение
            this.instance = instance;
        }

        // далее реализуем интерфейс IChangeVelocity
        // для get - берем значения соответствующих ключей в IUObject
        // для set - устанавливаем значение для соответствующих ключей

        public Vector Position
        {
            get => (Vector)this.instance.GetProperty("position");
            set => this.instance.SetProperty("position", value);
        }

        public Vector Velocity
        {
            get => (Vector)this.instance.GetProperty("velocity");
            set => this.instance.SetProperty("velocity", value);
        }
    }
}