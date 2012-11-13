using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Yavalath
{
	public class Board
	{
		public class Cell
		{
			public int Border {get; private set;}
			public bool Playable {get; private set;}
			public int Player {get; set;}

			/// <summary>
			/// Initializes a new instance of the <see cref="Yavalath.Board+Cell"/> class.
			/// </summary>
			/// <param name='border'>
			/// Integer input determining the type of cell
			/// </param>
			/// <param name='playable'>
			/// Integer indicating if cell is playable or not
			/// </param>
			/// <param name='player'>
			/// Indicates which player takes up the cell
			/// </param>
			public Cell (int border, bool playable = false, int player = 0)
			{
				Border = border;
				Playable = playable;
				Player = player;
			}

			public char PieceSymbol ()
			{
				if (Player == 0) return ' ';
				else if (Player == -1) return 'X';
				return 'O';
			}
		}

		private Cell[][] Cells = new Cell[][]{
			new Cell[11],
			new Cell[11],
			new Cell[11],
			new Cell[11],
			new Cell[11],
			new Cell[11],
			new Cell[11],
			new Cell[11],
			new Cell[11],
			new Cell[11],
			new Cell[11]
		};

		private static int[][] Borders = new int[][] {
			new int[] { 0, 0, 0, 0, 0, 1, 3, 3, 3, 3, 2 },
			new int[] { 0, 0, 0, 0, 5, 7, 7, 7, 7, 7, 2 },
			new int[] { 0, 0, 0, 5, 7, 7, 7, 7, 7, 7, 2 },
			new int[] { 0, 0, 5, 7, 7, 7, 7, 7, 7, 7, 2 },
			new int[] { 0, 5, 7, 7, 7, 7, 7, 7, 7, 7, 2 },
			new int[] { 4, 7, 7, 7, 7, 7, 7, 7, 7, 7, 0 },
			new int[] { 4, 7, 7, 7, 7, 7, 7, 7, 7, 0, 0 },
			new int[] { 4, 7, 7, 7, 7, 7, 7, 7, 0, 0, 0 },
			new int[] { 4, 7, 7, 7, 7, 7, 7, 0, 0, 0, 0 },
			new int[] { 4, 7, 7, 7, 7, 7, 0, 0, 0, 0, 0 },
			new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
		};

		private static int[][] PlayableCells = new int[][] {
			new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			new int[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0 },
			new int[] { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0 },
			new int[] { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0 },
			new int[] { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
			new int[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
			new int[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
			new int[] { 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
			new int[] { 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
			new int[] { 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
			new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
		};

		public Board ()
		{
			int i = 0, j = 0;
			for (i = 0; i < 11; i++) {
				for (j = 0; j < 11; j++) {
					Cells[i][j] = new Cell(Borders[i][j], PlayableCells[i][j] == 1);
				}
			}
		}

		/// <summary>
		/// Output the current state of the Board
		/// </summary>
		public void Print ()
		{
			Func<int, int> indent = delegate(int x){while(x-- > 0) Console.Write(" "); return 0;}; 

			var indentation = 0;
			foreach (var row in Cells) {
				indent(indentation);
				Console.Write("  ");
				foreach (var cell in row) {
					Console.Write(cell.PieceSymbol());
					Console.Write(' ');
					Console.Write(new int[]{ 4, 5, 6, 7 }.Contains(cell.Border) ? "| " : " ");
					Console.Write(' ');
				}
				Console.WriteLine();
				indent(indentation);
				Console.Write(" ");
				foreach (var cell in row) {
					Console.Write(new int[]{ 2, 3, 6, 7 }.Contains(cell.Border) ? '\\' : ' ');
					Console.Write(' ');
					Console.Write(new int[]{ 1, 3, 5, 7 }.Contains(cell.Border) ? '/' : ' ');
					Console.Write(' ');
				}
				Console.WriteLine();
				indentation += 2;
			}
			Console.WriteLine("Next is ...");

		}

		public Cell[] this [int row] {
			get {
				return Cells [row];
			}
			private set {}
		}

		public bool TakeCell (string cellCoords = "", int player = 0)
		{
			if (cellCoords.Length < 2 || cellCoords.Length > 3)
				return false;

			try {
				var row = cellCoords.Substring (0, 1).ToUpper ().ElementAt (0) - 'A';
				var col = Convert.ToInt32 (cellCoords.Substring (1));

				Cell cell = Cells[row].SelectMany(c => c.Playable)[col];

				if(cell.Player != 0) return false;
				cell.Player = player;

				return true;
			} catch (Exception e) {
				Console.Write("Error... YAY");
				return false;
			}

			return false;
		}

		static void Main ()
		{
			new Board().Print();
		}
	}
}

