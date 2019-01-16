using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TicTacToeLib;

namespace TicTacToeLib
{
    public static class Teacher
    {
        private static Random random = new Random();

        public static IDictionary<PlayerMark, ITicTacToeAi> Teach(int countInRow, int boardWidth, int boardHeight, int gameCount, double alpha, double gamma, Action<int, int> progressNotifier = null) 
        {
            IDictionary<PlayerMark, ITicTacToeAi> ticTacToeAiMap = new Dictionary<PlayerMark, ITicTacToeAi>();

            foreach (PlayerMark playerMark in Enum.GetValues(typeof(PlayerMark)))
            {
                ticTacToeAiMap.Add(playerMark, new TicTacToeAi(alpha, gamma, boardWidth, boardHeight, countInRow, playerMark));
            }

         //   ticTacToeAiMap.Add(PlayerMark.Nought, new TicTacToeAi(alpha, gamma, boardWidth, boardHeight, countInRow, PlayerMark.Nought));
         //   ticTacToeAiMap.Add(PlayerMark.Cross, new AI());

            for (int i = 0; i < gameCount; i++)
            {
                progressNotifier?.Invoke(i, gameCount);

                Game game = new Game(countInRow, boardWidth, boardHeight, default(PlayerMark));

                while (!game.Completed)
                {
                    Point2D point = GetTurn(ticTacToeAiMap, game);
                    game.DoTurn(point.X, point.Y);
                }

                // Console.WriteLine("Winner: {0}", game.Winner?.ToString() ?? "Tie");

                PlayerMark? winner = game.Winner;
                double reward = winner.HasValue ? 1.0 : 0.0;

                Board board = game.Board.GetCopy();
                PlayerMark? currentPlayerNullable = board.LastPlayerMark;

                do
                {                    
                    Point2D point2D = board.LastPoint;
                    board.Undo();
                    PlayerMark currentPlayer = currentPlayerNullable.Value;
                    double playerReward = winner == currentPlayer ? reward : -reward;
                    ticTacToeAiMap[currentPlayer].Backpropagate(board, point2D, playerReward);
                    currentPlayerNullable = board.LastPlayerMark;
                } while (currentPlayerNullable.HasValue);
            }

            return ticTacToeAiMap;            
        }

        private static Point2D GetTurn(IDictionary<PlayerMark, ITicTacToeAi> ticTacToeAiMap, Game game)
        {
            if (random.NextDouble() < 0.1)
            {
                return new TicTacToeLib.Point2D(random.Next(game.BoardWidth), random.Next(game.BoardHeight));
            }

            return ticTacToeAiMap[game.CurrentPlayer].GetTurn(game.Board, game.CurrentPlayer);
        }
    }
}
