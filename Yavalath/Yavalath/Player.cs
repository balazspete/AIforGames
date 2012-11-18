using System;

namespace Yavalath
{
	public abstract class Player
	{
        public string Name {get; protected set;}

		public abstract string GetNextMove();
	}

    public class HumanPlayer : Player
    {
        public HumanPlayer(string name)
        {
            this.Name = name;
        }

        public override string GetNextMove()
        {
            Console.Write("Take your move: [CellCoords] or \"TakeOver\"");
            var input = Console.ReadLine();
            return input;
        }
    }

    public class ComputerPlayer : Player
    {
        public ComputerPlayer(string name)
        {
            this.Name = name;
        }

        public override string GetNextMove()
        {
            return null;
        }
    }
}

