using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeLib;

namespace TicTacToeTrain
{
    class Program
    {
        const int _CountInRow = 3, _BoardWidth = 3, _BoardHeight = 3, _GameCount = 10000;
        const double _Alpha = 0.9, _Gamma = 1;

        static void Main(string[] args)
        {
            Teacher.Teach(_CountInRow, _BoardWidth, _BoardHeight, _GameCount, _Alpha, _Gamma, (current, total) => Console.WriteLine("{0}/{1}", current, total));
        }
    }
}
