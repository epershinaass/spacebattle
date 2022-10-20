using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpaceBattle.Move
{
    public interface IMovable
    {
        public System.Numerics.Vector Position
        {
            get;
            set;
        }
        public System.Numerics.Vector Velosity
        {
            get;
        }
    }
}
