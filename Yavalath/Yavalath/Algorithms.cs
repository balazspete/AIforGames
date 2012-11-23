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
					pattern = new int[]{ 0, player, player, 0, player },
					weight = 10 },
				new Pattern {
					pattern = new int[]{ 0, player, 0, 0, player },
					weight = 20 },
				new Pattern {
					pattern = new int[]{ player, 0, player, player, 0 },
					weight = 10 },
				new Pattern {
					pattern = new int[]{ player, 0, 0, player, 0 },
					weight = 20 },
				new Pattern {
					pattern = new int[]{ player, player, player, player },
					weight = 1000 }};
		}

		public static double Evaluation (Board gameBoard, Cell cell, int playerSymbol, Pattern[] patterns = null)
		{
			Func<Cell, int> getPlayer = c => c.Player;
			var lines = new int[][]{
				gameBoard.UpDiagonal (cell).Select (getPlayer).ToArray (),
				gameBoard.DownDiagonal (cell).Select (getPlayer).ToArray (),
				gameBoard.Row (cell).Select (getPlayer).ToArray ()
			};

			Func<double> getScore = delegate {
				double result = 0;
				foreach (var line in lines) {
					if(line.Where (e => e!=0).Count()==0) continue;
					foreach (var pattern in patterns) {
						var p = pattern.pattern;
						for (var i = 0; i < line.Length - p.Length; i++) {
							var subsequence = line.Skip(i).Take(p.Length);
							if (subsequence.SequenceEqual(p)) {
								Console.WriteLine ("Match {0}", string.Join (",", subsequence));
								result += pattern.weight;
							}
						}
					}
				}
				return result;
			};

			if(patterns == null) patterns = Patterns(playerSymbol);

			var t = cell.Player;
			cell.Player = 0;

			var preScore = getScore();
			cell.Player = t;
			var postScore = getScore();

			Console.WriteLine ("Eval {0} {1}", postScore, preScore);
            return postScore - preScore;
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



//		public static SearchResult Negamax (Board board, int player, int height, 
//			Cell cell = null, double oldscore = 0, Pattern[] patterns = null)
//		{
//			var emptyCells = board.EmptyCells ();
//
//
//			if (height == 0 || emptyCells.Length == 0) {
//				return new SearchResult {
//					Score = oldscore + Evaluation(board, cell, player, patterns),
//					Cell = cell
//				};
//			} else {
//				SearchResult temp, score = new SearchResult { Score=-1.0/0, Cell = null };
//				foreach (var _cell in emptyCells) {
//					_cell.Player = player;
//					var _score = oldscore + (cell != null ? Evaluation(board, cell, player, patterns) : 0);
//					temp = -Negamax(board, -player, height-1, _cell, _score, patterns);
//					_cell.Player = 0;
//					if(score.Cell == null && temp.Cell != null) score = temp;
//					if(score.Score <= temp.Score) 
//						score = temp;
//				}
//				return score;
//			}
//		}

		public static int Negamax(Board board, Cell cell, int player)
		{

		}

    }
}

