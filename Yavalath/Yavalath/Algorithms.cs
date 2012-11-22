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
            public int weight;
        }

		public static int Evaluation (Board gameBoard, int player, Pattern[] patterns = null)
		{
			var result = 0;

			if(patterns == null)
				patterns = new Pattern[] {
					new Pattern {
						pattern = new int[]{ 0, player, player, 0 }, 
						weight = -2 },
					new Pattern {
						pattern = new int[]{ 0, player, 0, player, 0 },
						weight = -1 },
					new Pattern {
						pattern = new int[]{ 0, player, player, 0, player },
						weight = 2 },
					new Pattern {
						pattern = new int[]{ 0, player, 0, 0, player },
						weight = 2 },
					new Pattern {
						pattern = new int[]{ player, 0, player, player, 0 },
						weight = 2 },
					new Pattern {
						pattern = new int[]{ player, 0, 0, player, 0 },
						weight = 2 }};

			var cellsss = new Board.Cell[][][] {
            	gameBoard.Rows (),
            	gameBoard.UpDiagonals (),
            	gameBoard.DownDiagonals () };

			Func<int[], int[], bool> identical = delegate(int[] a, int[] b) {
				if (a.Length != b.Length){
					return false;
				}
				return a == b;
			};

			Func<Board.Cell[][], Pattern, int> occurences = delegate(Board.Cell[][] cellss, Pattern p) {
				var count = 0;
				foreach (var cells in cellss) {
					var q = cells.Where (c => c.Playable).Select (c => c.Player).ToArray ();
					for (var i = 0; i < q.Length - p.pattern.Length; i++) {
						var _q = q.Skip (i).Take (p.pattern.Length).ToArray();
						if (identical (_q, p.pattern)){
							count++;
						}
					}
				}
				return count;
			};

			foreach (var p in patterns) {
				foreach(var c in cellsss)
				{
					result += occurences(c, p) * p.weight;
				}
			}

            return result;
        }


		public struct SearchResult
		{
			public double Score;
			public Board.Cell Cell;
		}

		public static SearchResult Negamax (Board board, Board.Cell cell, int player, int height, Pattern[] patterns = null)
		{
			var emptyCells = board.EmptyCells ();
			if (height == 0 || emptyCells.Length == 0) {
				return new SearchResult {Score=Evaluation (board, player, patterns), Cell=cell};
			} else {
				SearchResult temp, score = new SearchResult {Score=-1.0/0, Cell = null};
				foreach(var c in emptyCells)
				{
					double _temp, _t;
					c.Player = player;
					temp = Negamax(board, c, -1 * player, height-1, patterns);
					temp.Score = -temp.Score;
					c.Player = 0;
					_t = Math.Max(score.Score, temp.Score);
					if(_t != score.Score)
						score = temp;
				}
				return score;
			}
		}
    }
}

