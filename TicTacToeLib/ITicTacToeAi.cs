using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeLib
{
    public interface ITicTacToeAi
    {
       Point2D GetTurn(IBoard board, PlayerMark currentPlayer);

       void Backpropagate(Board board, Point2D point, double reward);
    }
}
