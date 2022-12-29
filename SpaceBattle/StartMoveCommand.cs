using SpaceBattle;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle
{
    public class StartMoveCommand : ICommand
    {

        // объект, который будем двигать
        private IMoveCommandStartable obj;
        public StartMoveCommand(IMoveCommandStartable obj) 
        {
            this.obj = obj;
        }
        // очередь команд, которую извлечем из приказа
        private Queue<ICommand> queue;

        
        public void Execute()
        {
            IoC.Resolve<ICommand>("SpaceBattle.SetupProperty", obj.Obj, "Velocity", obj.InitialVelocity).Execute();
            // создание MoveCommand
            var moveCommand = IoC.Resolve<ICommand>("SpaceBattle.Move", obj.Obj);
            IoC.Resolve<ICommand>("SpaceBattle.SetupCommand", obj.Obj, "Move", moveCommand).Execute();
            var createQueue = IoC.Resolve<Queue<ICommand>>("SpaceBattle.Queue");
            IoC.Resolve<ICommand>("SpaceBattle.QueuePush", createQueue, moveCommand).Execute();
        }
    }
}