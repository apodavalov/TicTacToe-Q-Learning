using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeLib
{
    public interface IBoard
    {
        int CountInRow
        {
            get;
        }

        int Width
        {
            get;
        }

        int Height
        {
            get;
        }

        Cell this[int x, int y]
        {
            get;
        }

        Board GetCopy();
    }
}
