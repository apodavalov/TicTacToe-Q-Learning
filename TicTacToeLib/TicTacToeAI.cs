using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeLib;

namespace TicTacToeLib
{
    public class TicTacToeAi : ITicTacToeAi
    {
        private readonly double _Alpha;
        private readonly double _Gamma;
        private readonly int _Width;
        private readonly int _Height;
        private readonly int _CountInRow;
        private readonly PlayerMark _PlayerMark;
        private double[,] _StateActionTable;

        public TicTacToeAi(double alpha, double gamma, int width, int height, int countInRow, PlayerMark playerMark)
        {
            _Alpha = alpha;
            _Gamma = gamma;
            _Width = width;
            _Height = height;
            _PlayerMark = playerMark;
            _CountInRow = countInRow;

            int stateCount = (int)Math.Pow(GetCellStateCount(), _Width * _Height);
            int actionCount = _Width * _Height;

            _StateActionTable = new double[stateCount, actionCount];
        }

        private static int GetCellStateCount()
        {
            return Enum.GetValues(typeof(PlayerMark)).Length + 1;
        }

        public Point2D GetTurn(IBoard board, PlayerMark currentPlayer)
        {
            if (board.Width != _Width || board.Height != _Height)
            {
                throw new InvalidOperationException();
            }

            return GetAction(board);
        }

        private Point2D ActionToPoint2D(int action)
        {
            return new Point2D(action % _Width, action / _Width);
        }

        private int Point2DToAction(Point2D point)
        {
            return point.Y * _Width + point.X;
        } 

        private Point2D GetAction(IBoard board)
        {
            int state = ToStateNumber(board);

            int index = -1;

            for (int i = 0; i < _StateActionTable.GetLength(1); i++) {
                double newValue =_StateActionTable[state, i];

                if (index < 0 || _StateActionTable[state, index] < newValue)
                {
                    Point2D point = ActionToPoint2D(i);

                    if (board[point.X, point.Y].Empty)
                    {
                        index = i;
                    }
                }
            }
            
            return ActionToPoint2D(index);
        }

        private int ToStateNumber(IBoard board)
        {
            int cellStateCount = GetCellStateCount();

            int result = 0;

            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Height; j++)
                {
                    result = result * GetCellStateCount() + GetNumberByCell(board[i, j]);
                }
            }

            return result;
        }

        private int GetNumberByCell(Cell cell)
        {
            if (cell.Empty)
            {
                return 1;
            }
            
            switch (cell.PlayerMark)
            {
                case PlayerMark.Cross:
                    return 0;
                case PlayerMark.Nought:
                    return 2;
                default:
                    throw new InvalidOperationException();
            }
        }

        public void Backpropagate(Board board, Point2D point, double reward)
        {
            int state = ToStateNumber(board);
            int action = Point2DToAction(point);
            double maxQ = GetMaxQ(state);

            _StateActionTable[state, action] = 
                (1.0 - _Alpha) * _StateActionTable[state, action] +
                _Alpha * (reward + _Gamma * maxQ);
        }

        private double GetMaxQ(int oldState)
        {
            double? maxValue = null;

            for (int i = 0; i < _StateActionTable.GetLength(1); i++)
            {
                double value = _StateActionTable[oldState, i];
                if (!maxValue.HasValue || value > maxValue)
                {
                    maxValue = value;
                }
            }

            return maxValue.Value;
        }
    }
}
