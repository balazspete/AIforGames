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
		private bool KillerHeuristic;

		public ComputerPlayer (string name, int searchDepth, 
			Algorithms.Pattern[] patterns = null, bool killerHeuristic = false)
		{
			Name = name;
			SearchDepth = searchDepth;
			Patterns = patterns;
			KillerHeuristic = killerHeuristic;
		}

//		public static SearchResult Negamax (Board board, Cell cell, int player, int height, 
//		                                    int oldscore = 0, Pattern[] patterns = null)
        public override string GetNextMove(Board board, int playerSymbol)
        {
			Score -= board.Latest.Score;
			var s = Algorithms.ABNegamax(board, board.Latest.Cell, SearchDepth, 
				new Algorithms.SearchResult {Score = -1.0/0, Count = 0}, 
				new Algorithms.SearchResult {Score = 1.0/0}, playerSymbol);

			Score = s.Score;
			Console.WriteLine("Position: {0}; Evaluation count: {1}", s.Cell.Position, s.Count);
			return s.Cell.Position;
        }
    }
}

