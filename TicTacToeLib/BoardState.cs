using System;

namespace TicTacToeLib
{
    class BoardState
    {
        public Point2D Point
        {
            get;
            set; 
        }

        public int[] MaxCountInRowPerMark
        {
            get;
            private set;
        }

        public BoardState()
        {
            MaxCountInRowPerMark = new int[Enum.GetValues(typeof(PlayerMark)).Length];
        }

        public BoardState GetCopy()
        {
            int[] maxCountInRowPerMark = new int[MaxCountInRowPerMark.Length];
            Point2D point = Point;
            Array.Copy(MaxCountInRowPerMark, maxCountInRowPerMark, MaxCountInRowPerMark.Length);

            return new BoardState() { Point = point, MaxCountInRowPerMark = maxCountInRowPerMark };
        }
    }
}
