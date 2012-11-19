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

        /// <summary>
        /// Initializes a new instance of the <see cref="Yavalath.Game"/> class.
        /// </summary>
        /// <param name='player1'>
        /// Player1.
        /// </param>
        /// <param name='player2'>
        /// Player2.
        /// </param>
		public Game (Player player1, Player player2)
		{
			GameBoard = new Board();
            Players = new Dictionary<Player, int>(){
                {player1, -1},
                {player2, 1}
            };
            
            CurrentPlayer = player1;
		}

        /// <summary>
        /// Run the game.
        /// </summary>
        public void Run()
        {
            var round = 0;
            bool? result = null;

            while(true)
            {
                round++; 
                while(!GameBoard.TakeCell(CurrentPlayer.GetNextMove(), Players[CurrentPlayer], round == 2)) 
                    Console.WriteLine("Invalid move, please try again...");

                GameBoard.Print();

                if((result = HasWonOrLost()) != null)
                {
                    var winner = Players
                        .Where(p => p.Value == (result.Value ? 1 : -1) * Players[CurrentPlayer]).ToArray();
                    Console.WriteLine(
                        "\n{0} has won the game!", 
                        winner.First().Key.Name);
                    return;
                }

                NextPlayer();
                Console.WriteLine("\nIt's {0}'s turn...", CurrentPlayer.Name);

            }
        }

        /// <summary>
        /// Selects the next the player.
        /// </summary>
        private void NextPlayer() 
        {
            var next = Players.Where(p => p.Value != Players[CurrentPlayer]).Select(p => p.Key).First();
            CurrentPlayer = next;
        }

        /// <summary>
        /// Determines whether the last move caused the player to win or loose
        /// </summary>
        /// <returns>
        /// <c>true</c> if the player has won; <c>false</c> if the player has lost; otherwise <c>null</c>
        /// </returns>
        public bool? HasWonOrLost()
        {
            var latest = GameBoard.Latest;
            var count = 1;

            var diagonal1 = CheckNeighbour(latest.Index, -11) + CheckNeighbour(latest.Index, 11);
            var diagonal2 = CheckNeighbour(latest.Index, -10) + CheckNeighbour(latest.Index, 10);
            var horizontal = CheckNeighbour(latest.Index, -1) + CheckNeighbour(latest.Index, 1);

            Console.WriteLine("{0} {1} {2}", diagonal1, diagonal2, horizontal);
            count += new int[]{diagonal1, diagonal2, horizontal}.Max();

            Console.WriteLine("{0} ", count);
            if (count >= 4) return true;
            else if (count == 3) return false;
            else return null;
        }

        /// <summary>
        /// Recursively checks the neighbouring cells to count the length of the subsequence of cells belonging to the player.
        /// </summary>
        /// <returns>
        /// The length of the sequence
        /// </returns>
        /// <param name='index'>
        /// Starting cell index
        /// </param>
        /// <param name='offset'>
        /// Offset.
        /// </param>
        private int CheckNeighbour(int index, int offset)
        {
            var count = 0;
            
            var newIndex = index + offset;
            if (0 <= index && index < 121 && 0 <= newIndex && newIndex < 121)
            {
                var cell1 = GameBoard [index];
                var cell2 = GameBoard [newIndex];
                var latest = GameBoard.Latest.Cell.Player;

                if (cell1.Playable && cell2.Playable && 
                     latest == cell1.Player && latest == cell2.Player)
                {
                    count++;
                    count += CheckNeighbour(newIndex, offset);
                }
            }

            return count;
        }

        
        static void Main ()
        {
            var game = new Game(new HumanPlayer("1"), new HumanPlayer("2"));
            game.Run();
        }
	}
}

