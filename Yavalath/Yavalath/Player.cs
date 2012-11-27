using System;

namespace Yavalath
{
	public abstract class Player
	{
		public double Score = 0;
        public string Name {get; protected set;}

		public abstract string GetNextMove(Board board = null, int playerSymbol = 0);
	}

    public class HumanPlayer : Player
    {
        public HumanPlayer(string name)
        {
            this.Name = name;
        }

		public override string GetNextMove(Board board, int playerSymbol)
        {
            Console.Write("Take your move: [CellCoords] or \"TakeOver\"");
            var input = Console.ReadLine();
            return input;
        }
    }

    public class ComputerPlayer : Player
    {
		private Algorithms.Pattern[] Patterns;
		private int SearchDepth;

		public ComputerPlayer (string name, int searchDepth, Algorithms.Pattern[] patterns = null)
		{
			Name = name;
			SearchDepth = searchDepth;
			Patterns = patterns;
		}

//		public static SearchResult Negamax (Board board, Cell cell, int player, int height, 
//		                                    int oldscore = 0, Pattern[] patterns = null)
        public override string GetNextMove(Board board, int playerSymbol)
        {
			Score -= board.Latest.Score;
			var s = Algorithms.Minimax(board, board.Latest.Cell, 
			                           SearchDepth, true, playerSymbol, Score);
			Score = s.Score;
			Console.WriteLine(s.Cell.Position);
			return s.Cell.Position;
			//return "";
        }
    }
}

