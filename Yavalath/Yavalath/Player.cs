using System;

namespace Yavalath
{
	public abstract class Player
	{
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

        public override string GetNextMove(Board board, int playerSymbol)
        {
			var s = Algorithms.Negamax(board, null, playerSymbol, SearchDepth, Patterns).Cell.Position;
			Console.WriteLine(s);
			return s;
        }
    }
}

