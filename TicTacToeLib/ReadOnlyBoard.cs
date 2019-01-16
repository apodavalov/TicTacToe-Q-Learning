using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeLib
{
    public class ReadOnlyBoard : IBoard
    {
        private Board _Board;

        public int CountInRow
        {
            get
            {
                return _Board.CountInRow;
            }
        }

        public int Width
        {
            get
            {
                return _Board.Width;
            }
        }

        public int Height
        {
            get
            {
                return _Board.Height;
            }
        }

        public Cell this[int x, int y]
        {
            get
            {
                return _Board[x, y];
            }
        }

        public Board GetCopy()
        {
            return _Board.GetCopy();
        }

        public ReadOnlyBoard(Board board)
        {
            _Board = board;
        }
    }
}
