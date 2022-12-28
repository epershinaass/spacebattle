using SpaceBattle.Adapters;
using SpaceBattle.Move;

namespace SpaceBattle
{
    public class StartMoveCommand : ICommand
    {

        // объект, который будем двигать
        private IMovable movable;
        // очередь команд, которую извлечем из приказа
        private Queue<ICommand> queue;

        public StartMoveCommand(IMoveCommandStartable startable)
        {
            // получаем объект, который нужно двигать (по ключу object в универсальном объекте, который находится в приказе)
            // сам приказ - член интерфейса IMoveCommandStartable
            // преобразуем полученный объект типа Object в объект интерфейса IChangeVelocity (через адаптер)
            // это работает, потому что MovableAdapter реализует интерфейс IChangeVelocity
            this.movable = new MovableAdapter((IUObject)startable.Order.GetProperty("object"));
            // аналогично получаем очередь команд, воспринимаем ее как объект по ключу queue
            this.queue = (Queue<ICommand>)startable.Order.GetProperty("queue");
            // пользуемся возможностью интерфейса IChangeVelocity и соответствующим адаптером, чтобы изменить скорость
            // при этом, саму скорость также "достаем" из приказа (по ключу velocity)
            this.movable.Velocity = (Vector)startable.Order.GetProperty("velocity");
        }

        public void Execute()
        {
            // создание MoveCommand
            var moveCommand = new MoveCommand(movable);
            // вызов MoveCommand
            moveCommand.Execute();
            // добавляем команду в очередь
            this.queue.Enqueue(moveCommand);
        }
    }
}