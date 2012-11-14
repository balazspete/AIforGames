using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Yavalath
{
	public class Game
	{
		private Board GameBoard;
        public Player CurrentPlayer { get; private set; }

        private IDictionary<Player, int> Players;

		public Game (Player player1, Player player2)
		{
			GameBoard = new Board();
            Players = new Dictionary<Player, int>(){
                {player1, -1},
                {player2, 1}
            };

            CurrentPlayer = player1;
		}

        private void NextPlayer() 
        {
            var next = Players.Where(p => p.Value != Players[CurrentPlayer]).Select(p => p.Key);
            CurrentPlayer = next;
        }
	}
}

