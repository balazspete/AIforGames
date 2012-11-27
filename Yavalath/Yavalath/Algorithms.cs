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

			public static SearchResult operator -(SearchResult sr){
				sr.Score *= -1;
				return sr;
			}
		}

		/// <summary>
		/// Select the next cell using MiniMax search
		/// </summary>
		/// <param name='board'>
		/// The Game Board.
		/// </param>
		/// <param name='cell'>
		/// The last chosen cell.
		/// </param>
		/// <param name='height'>
		/// the height to which the search is ran.
		/// </param>
		/// <param name='maxing'>
		/// If set to <c>true</c> maximising the score.
		/// </param>
		/// <param name='player'>
		/// The Player.
		/// </param>
		/// <param name='originalScore'>
		/// Original score.
		/// </param>
		public static SearchResult Minimax (Board board, Cell cell, int height, 
		                                    bool maxing, int player, double originalScore = 0)
		{
			var emptyCells = board.EmptyCells ();
			if (height == 0 || emptyCells.Length == 0) {
				//return Evaluatio
				return new SearchResult {
					Score = originalScore + Evaluation (board, cell, player),
					Cell = cell
				};
			} else {
				SearchResult temp, score = new SearchResult {
					Score = maxing ? -1.0/0 : 1.0/0,
					Cell = null
				};
				foreach (var _cell in emptyCells) {
					_cell.Player = player;
					temp = Minimax (board, _cell, height-1, !maxing, player*-1);
					_cell.Player = 0;
					if (maxing) {
						if(temp.Score > score.Score) score = temp; 
					} else {
						if(temp.Score < score.Score) score = temp; 
					}
					if(height==3) Console.WriteLine(temp.Cell.Position);
				}
				//score.Cell = cell;
				score.Score += Evaluation (board, cell, player);
				return score;
			}
		}

    }
}

