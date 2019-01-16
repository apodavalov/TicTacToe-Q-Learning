using System.Collections.Generic;
using System;

namespace TicTacToeLib
{
    public class Cell
    {
        private static readonly Dictionary<PlayerMark, Cell> _Cells = GetAllValues();

        public static Cell EmptyCell
        {
            get;
            private set;
        } = new Cell();

        private static Dictionary<PlayerMark, Cell> GetAllValues()
        {
            Dictionary<PlayerMark, Cell> playerMarks = new Dictionary<PlayerMark, Cell>();

            foreach (PlayerMark playerMark in (PlayerMark[])Enum.GetValues(typeof(PlayerMark)))
            {
                playerMarks.Add(playerMark, new Cell(playerMark));
            }

            return playerMarks;
        }

        public bool IsMarked(PlayerMark playerMark)
        {
            return !Empty && playerMark == PlayerMark;
        }

        public bool Empty
        {
            get;
        }

        public PlayerMark PlayerMark
        {
            get;
        }

        private Cell(PlayerMark playerMark)
        {
            Empty = false;
            PlayerMark = playerMark;
        }

        private Cell()
        {
            Empty = true;
            PlayerMark = default(PlayerMark);
        }

        public static Cell Get(PlayerMark playerMark)
        {
            return _Cells[playerMark];
        }        
    }
}
