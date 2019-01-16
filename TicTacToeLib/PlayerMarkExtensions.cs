using System;

namespace TicTacToeLib
{
    public static class PlayerMarkExtensions
    {
        public static PlayerMark GetNext(this PlayerMark playerMark)
        {
            return (PlayerMark)(((int)playerMark + 1) % Enum.GetValues(typeof(PlayerMark)).Length);
        }

        public static PlayerMark GetPrev(this PlayerMark playerMark)
        {
            return (PlayerMark)(((int)playerMark + Enum.GetValues(typeof(PlayerMark)).Length - 1) % Enum.GetValues(typeof(PlayerMark)).Length);
        }
    }
}
