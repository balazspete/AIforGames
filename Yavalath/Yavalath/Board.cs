/*
 * Board.cs
 *
 * Balazs Pete
 * 09771417
 *
 */

using System;
using System.Linq;
using System.Data.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Yavalath
{
	public class Cell
	{
		public int Border {get; private set;}
		public bool Playable {get; private set;}
		public int Player {get; set;}
		public string Position {get; private set;}
		public int Index {get; private set;}

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
		public Cell (int border, bool playable = false, 
			int index = -1, string position = "", int player = 0)
		{
			Border = border;
			Playable = playable;
			Player = player;
			Position = position;
			Index = index;
		}

		/// <summary>
		/// Gets the symbol representation of the piece corresponding to the player
		/// </summary>
		/// <returns>
		/// The player's symbol character or ' '.
		/// </returns>
		public char PieceSymbol ()
		{
			if (Player == 0) return ' ';
			else if (Player == -1) return 'X';
			return 'O';
		}
	}

	public class Board
	{
		private const int DIMENSION = 11;
		private const int SIZE = 121;

        public struct CellIndex
        {
            public Cell Cell;
            public int Index;
			public double Score;
        }

		private Cell[] Cells = new Cell[SIZE];

		private static int[] Borders = new int[] {
			0, 0, 0, 0, 0, 1, 3, 3, 3, 3, 2,
			 0, 0, 0, 0, 5, 7, 7, 7, 7, 7, 2,
			  0, 0, 0, 5, 7, 7, 7, 7, 7, 7, 2,
			   0, 0, 5, 7, 7, 7, 7, 7, 7, 7, 2,
			    0, 5, 7, 7, 7, 7, 7, 7, 7, 7, 2,
			     4, 7, 7, 7, 7, 7, 7, 7, 7, 7, 0,
			      4, 7, 7, 7, 7, 7, 7, 7, 7, 0, 0,
			       4, 7, 7, 7, 7, 7, 7, 7, 0, 0, 0,
			        4, 7, 7, 7, 7, 7, 7, 0, 0, 0, 0,
			         4, 7, 7, 7, 7, 7, 0, 0, 0, 0, 0,
			          0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
		};

		private static int[] PlayableCells = new int[] {
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0,
			  0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0,
			   0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0,
			    0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0,
			     0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,
			      0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0,
			       0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
			        0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
			         0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0,
			          0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
		};

        /// <summary>
        /// Gets the last modified cell with its index.
        /// </summary>
        /// <value>
        /// The latest modified cell.
        /// </value>
        public CellIndex Latest {get; private set;}

        /// <summary>
        /// Initializes a new instance of the <see cref="Yavalath.Board"/> class.
        /// </summary>
		public Board ()
		{
			var count = 0;
			for(var i = 0; i < Cells.Length; i++) {
				var playable = PlayableCells[i] == 1;
				if(i % DIMENSION == 0) count = 1;
				if(playable) count++;
				Cells[i] = new Cell(Borders[i],
					playable: playable,
					index: i,
				    position: playable ? 
						String.Format("{0}{1}", (char)('A'-1+(i/DIMENSION)), count-1) : ""
				);
			}
		}

		/// <summary>
		/// Output the current state of the Board
		/// </summary>
		public void Print()
        {
            Func<int, int> indent = delegate(int x)
            {
                while (x-- > 0)
                    Console.Write(" ");
                return 0;
            };

            var indentation = 0;
			var row = 0;
			while(row < DIMENSION)
            {
                indent(indentation);
                Console.Write("  ");

				var cellsInRow = Cells.Skip(row * DIMENSION).Take(DIMENSION);

                foreach (var cell in cellsInRow)
                {
                    var symbol = cell.PieceSymbol();
                    Console.Write(symbol);
                    Console.Write(' ');
                    Console.Write(new int[]{ 4, 5, 6, 7 }.Contains(cell.Border) ? "|" : " ");
                    Console.Write(' ');
                }
                Console.WriteLine();
                indent(indentation);
                Console.Write(" ");
                foreach (var cell in cellsInRow)
                {
                    Console.Write(new int[]{ 2, 3, 6, 7 }.Contains(cell.Border) ? '\\' : ' ');
                    Console.Write(' ');
                    Console.Write(new int[]{ 1, 3, 5, 7 }.Contains(cell.Border) ? '/' : ' ');
                    Console.Write(' ');
                }
                Console.WriteLine();
                indentation += 2;
				row++;
            }
		}

        /// <summary>
        /// Gets the <see cref="Yavalath.Board"/> at the specified index.
        /// </summary>
        /// <param name='index'>
        /// Index.
        /// </param>
        public Cell this [int index]
        {
            get
            {
				if(index < 0 || index >= SIZE) return null;
                return Cells[index];
            }
        }

		public Cell this [int row, int column]
		{
			get
			{
				return this [row * DIMENSION + column];
			}
		}

		public Cell[] EmptyCells ()
		{
			return Cells.Where(c => c.Playable && c.Player == 0).ToArray();
		}

		public List<Cell> UpDiagonal(Cell cell)
		{
			return UpDiagonal(cell.Index);
		}

		public List<Cell> UpDiagonal (int cellIndex)
		{
			return line (cellIndex, 10);
		}

		public List<Cell> DownDiagonal (Cell cell)
		{
			return UpDiagonal(cell.Index);
		}

        public List<Cell> DownDiagonal (int cellIndex)
		{
			return line (cellIndex, -11); 
		}

		public List<Cell> Row (Cell cell)
		{
			return Row(cell.Index);
		}

		public List<Cell> Row (int cellIndex)
		{
			return Cells.Skip((cellIndex/DIMENSION) * DIMENSION).Take(DIMENSION).ToList ();
		}

        private List<Cell> line (int index, int offset)
		{
			var column = index % DIMENSION;
			var row = index / DIMENSION;
			offset = Math.Abs (offset);

			var columnOffset = offset == 10 ? 1 : 0;

			var list = new List<Cell>();

			while (0 < row && column < DIMENSION) {
				row--;
				column += columnOffset;
			}

			while (row < DIMENSION && column >= 0) {
				list.Add (this[row, column]);
				row++;
				column -= columnOffset;
			}

			return list;
        }

        /// <summary>
        /// Takes the cell.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if cell was taken, <c>false</c> otherwise.
        /// </returns>
        /// <param name='cellCoords'>
        /// Cell coords.
        /// </param>
        /// <param name='player'>
        /// Player.
        /// </param>
		public bool TakeCell(string cellCoords, int player = 0, bool takeOver = false)
        {
            if (cellCoords.Length < 2 || cellCoords.Length > 3)
				return false;

			try {
				var row = cellCoords.Substring (0, 1).ToUpper ().ElementAt (0) - 'A' + 1;
				var col = Convert.ToInt32 (cellCoords.Substring (1)) -1;

				var cells = Cells.Skip(row * DIMENSION).Take(DIMENSION);
                var playableCells = cells.Where(c => c.Playable).ToArray();
                Cell cell = playableCells.ElementAt(col);

				if(cell.Player != 0 && !takeOver) return false;

				cell.Player = player;
                Latest = new CellIndex {
					Cell = cell, 
					Index = row*11 + (10 - playableCells.Length + col),
					Score = Algorithms.Evaluation(this, cell, player, null)
				};

				return true;
			} catch (Exception) {
				Console.WriteLine("Error... YAY");
				return false;
			}
		}
	}
}

