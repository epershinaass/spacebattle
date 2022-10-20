using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpaceBattle.Move
{
    public class MoveCommand: ICommand
    {
        private IMovable=movable;
        public MoveCommand(IMovable movable)
        {
            this.movable = movable;
        }

        public void Execute()
        {
            movable.Position += movable.Velocity;
        }
    }
}