using System;

namespace TicTacToeLib
{
    public class AI : ITicTacToeAi
    {
        public static void DoTurn(Game game, int depth)
        {
            PlayerMark playerMark = game.CurrentPlayer;

            Board board = game.Board.GetCopy();

            AIRunner ai = new AIRunner(board, playerMark);
            PointWithMark point = ai.GetBest(1.0, depth);
            game.DoTurn(point.Point.X, point.Point.Y);            
        }

        public Point2D GetTurn(IBoard gameBoard, PlayerMark currentPlayer)
        {
            PlayerMark playerMark = currentPlayer;
            Board board = gameBoard.GetCopy();

            AIRunner aiRunner = new AIRunner(board, playerMark);
            return aiRunner.GetBest(1.0, board.Width * board.Height).Point;
        }

        public void Backpropagate(Board board, Point2D point, double reward)
        {
            
        }
    }

    class AIRunner
    {
        private readonly Board _Board;
        private PlayerMark _PlayerMark;

        public AIRunner(Board board, PlayerMark playerMark)
        {
            _Board = board;
            _PlayerMark = playerMark;
        }

        private double GetWeight(double weight, int depth)
        {
            if (_Board.HasInRow(_PlayerMark))
            {
                return weight;
            }

            if (!_Board.HasEmpty() || depth == 0)
            {
                return 0.0;
            }

            PlayerMark oldPlayerMark = _PlayerMark;
            _PlayerMark = _PlayerMark.GetNext();
            PointWithMark pointWithMark = GetBest(weight / 2.0, depth - 1);
            _PlayerMark = oldPlayerMark;

            return -pointWithMark.Mark;
        }

        public PointWithMark GetBest(double weight, int depth)
        {
            PointWithMark pointWithMark = null;

            for (int i = 0; i < _Board.Width; i++)
            {
                for (int j = 0; j < _Board.Height; j++)
                {
                    if (_Board[i, j].Empty)
                    {
                        _Board.Mark(i, j, _PlayerMark);

                        double candidateWeight = GetWeight(weight, depth);

                        if (pointWithMark == null || pointWithMark.Mark < candidateWeight)
                        {
                            pointWithMark = new PointWithMark(new Point2D(i, j), candidateWeight);
                        }

                        _Board.Undo();
                    }
                }
            }

            return pointWithMark;
        }
    }
}
