using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Yavalath
{
    public class Algorithms
    {
        public struct Pattern
        {
            public int[] pattern;
            public double weight;
        }

		private static Pattern[] Patterns (int player)
		{
			return new Pattern[] {
				new Pattern {
					pattern = new int[]{ player, player, player },
					weight = -100 },
				new Pattern {
					pattern = new int[]{ 0, player, player, 0 },
					weight = -5 },
				new Pattern {
					pattern = new int[]{ 0, player, 0, player, 0 },
					weight = -2 },
				new Pattern {
					pattern = new int[]{ 0, player, 0, 0, player },
					weight = 10 },
				new Pattern {
					pattern = new int[]{ 0, player, player, 0, player },
					weight = 100 },
				new Pattern {
					pattern = new int[]{ player, 0, 0, player, 0 },
					weight = 10 },
				new Pattern {
					pattern = new int[]{ player, 0, player, player, 0 },
					weight = 100 },
				new Pattern {
					pattern = new int[]{ player, player, player, player },
					weight = 1000 }};
		}

		/// <summary>
		/// Evaluate the score for a specific <see cref="Yavalath.Cell"/>.
		/// </summary>
		/// <param name='gameBoard'>
		/// Game board.
		/// </param>
		/// <param name='cell'>
		/// Cell.
		/// </param>
		/// <param name='playerSymbol'>
		/// Player symbol.
		/// </param>
		/// <param name='patterns'>
		/// Patterns.
		/// </param>
		public static double Evaluation (Board gameBoard, Cell cell, int playerSymbol, Pattern[] patterns = null)
		{

			if(patterns == null) patterns = Patterns(playerSymbol);
			var patterns2 = Patterns (-playerSymbol);

			var t = cell.Player;
			cell.Player = 0;
			var lines = GetLines(gameBoard, cell);
			var preScore = GetScore(lines, patterns) + GetScore(lines, patterns2);

			cell.Player = t;
			lines = GetLines(gameBoard, cell);
			var postScore = GetScore(lines, patterns) + 2*GetScore(lines, patterns2);

            return postScore - preScore;
        }

		/// <summary>
		/// Gets the correcponding row and diagonals for a <see cref="Yavalath.Cell"/>.
		/// </summary>
		/// <returns>
		/// The player values for each cell.
		/// </returns>
		/// <param name='gameBoard'>
		/// The Game board.
		/// </param>
		/// <param name='cell'>
		/// The Cell.
		/// </param>
		private static int[][] GetLines (Board gameBoard, Cell cell)
		{
			Func<Cell, int> getPlayer = c => c.Player;
			var t = gameBoard.UpDiagonal (cell).Select (getPlayer).ToArray ();
			return new int[][]{
				gameBoard.UpDiagonal (cell).Select (getPlayer).ToArray (),
				gameBoard.DownDiagonal (cell).Select (getPlayer).ToArray (),
				gameBoard.Row (cell).Select (getPlayer).ToArray () };
		}

		/// <summary>
		/// Gets the score of a set of <see cref="Yavalath.Cell"/>s.
		/// </summary>
		/// <returns>
		/// The score.
		/// </returns>
		/// <param name='lines'>
		/// Set of <see cref="Yavalath.Cell"/>s.
		/// </param>
		/// <param name='patterns'>
		/// Custom <see cref="Yavalath.Pattern"/>s.
		/// </param>
		private static double GetScore (int[][] lines, Pattern[] patterns = null)
		{
			double result = 0;
			foreach (var line in lines) {
				if(line.Where (e => e!=0).Count()==0) continue;
				foreach (var pattern in patterns) {
					var p = pattern.pattern;
					for (var i = 0; i < line.Length - p.Length; i++) {
						var subsequence = line.Skip(i).Take(p.Length);
						if (string.Join (",", subsequence) == string.Join (",", p)) {
							//Console.WriteLine ("Match {0} {1}", string.Join (",", subsequence), string.Join (",", p));
							//Console.WriteLine(pattern.weight);
							result += pattern.weight;
						}
					}
				}
			}
			return result;
		}

		public struct SearchResult
		{
			public double Score;
			public Cell Cell;
			public int Count;

			public static SearchResult operator -(SearchResult sr){
				sr.Score *= -1;
				return sr;
			}
		}

		/// <summary>
		/// Execute Alpha - Beta Negemax search to find the next best cell.
		/// </summary>
		/// <returns>
		/// The best cell and its score.
		/// </returns>
		/// <param name='board'>
		/// The Game Board.
		/// </param>
		/// <param name='cell'>
		/// The last taken <see cref="Yavalath.cell"/>.
		/// </param>
		/// <param name='height'>
		/// The Height to which the search should be run until.
		/// </param>
		/// <param name='achievable'>
		/// The Achievable Cell.
		/// </param>
		/// <param name='hope'>
		/// Hope.
		/// </param>
		/// <param name='player'>
		/// Player.
		/// </param>
		public static SearchResult ABNegamax (Board board, Cell cell, int height, 
			SearchResult achievable, SearchResult hope, int player)
		{
			var emptyCells = board.EmptyCells ();
			if (height == 0 || emptyCells.Length == 0) {
				return new SearchResult {
					Score = Evaluation (board, cell, player),
					Cell = cell,
					Count = 1
				};
			} else {
				SearchResult temp;
				var score = Evaluation (board, cell, player);
				achievable.Count += 1;

				foreach (var _cell in emptyCells)
				{
					_cell.Player = player;
					temp = -ABNegamax (board, _cell, height-1, -hope, -achievable, -player);
					temp.Score += score;
					_cell.Player = 0;
					if (temp.Score >= hope.Score) {
						return temp;
					}
					achievable.Count = temp.Count = achievable.Count + temp.Count;
					achievable = temp.Score >= achievable.Score ? temp : achievable;
				}
				return achievable;
			}
		}
    }
}

