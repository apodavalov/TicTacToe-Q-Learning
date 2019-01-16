namespace TicTacToeLib
{
    public class Game
    {
        private readonly Board _Board;

        public ReadOnlyBoard Board
        {
            get;
            private set;
        }

        public PlayerMark CurrentPlayer
        {
            get;
            private set;
        }

        public int CountInRow
        {
            get
            {
                return _Board.CountInRow;
            }
        }

        public bool Completed
        {
            get;
            private set;
        }

        public PlayerMark? Winner
        {
            get;
            private set;
        }

        public Cell this[int x, int y]
        {
            get
            {
                return _Board[x, y];
            }
        }

        public int BoardWidth
        {
            get
            {
                return _Board.Width;
            }
        }

        public int BoardHeight
        {
            get
            {
                return _Board.Height;
            }
        }

        public Game(int countInRow, int width, int height, PlayerMark currentPlayer)
        {
            _Board = new Board(width, height, countInRow);
            Board = new ReadOnlyBoard(_Board);
            CurrentPlayer = currentPlayer;
        }

        public bool DoTurn(Point2D point)
        {
            return DoTurn(point.X, point.Y);
        }

        public bool DoTurn(int x, int y)
        {
            if (Completed || !_Board[x, y].Empty)
            {
                return false;
            }

            _Board.Mark(x, y, CurrentPlayer);

            if (_Board.HasInRow(CurrentPlayer))
            {
                Completed = true;
                Winner = CurrentPlayer;
            }

            if (!_Board.HasEmpty())
            {
                Completed = true;
            }

            CurrentPlayer = CurrentPlayer.GetNext();

            return true;
        }        
    }
}
