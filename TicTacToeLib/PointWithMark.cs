using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeLib
{
    class PointWithMark
    {
        public Point2D Point
        {
            get;
            private set;
        }
        
        public double Mark
        {
            get;
            private set;
        }

        public PointWithMark(Point2D point, double mark)
        {
            Point = point;
            Mark = mark;
        }
    }
}
