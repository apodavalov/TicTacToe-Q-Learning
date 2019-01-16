using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeLib
{
    public class Point2D
    {
        public int X
        {
            get;
            private set;
        }

        public int Y
        {
            get;
            private set;
        }
        
        public Point2D(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
