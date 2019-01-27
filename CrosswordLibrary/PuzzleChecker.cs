using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordLibrary
{
    public static class PuzzleChecker
    {
        //public PuzzleChecker(int size, Hashtable columnHashtable)
        //{
        //    Size = size;
        //    ColumnHashtable = columnHashtable;
        //    DefiningColumns = new Column[((size + 1) / 2)];

        //    Cells = new bool[Size][];

        //    for (int i = 0; i < Size; i++)
        //    {
        //        Cells[i] = new bool[Size];
        //        for (int j = 0; j < Size; j++)
        //        {
        //            Cells[i][j] = true;
        //        }
        //    }
        //}

        //public int Size { get; private set; }
        //public Hashtable ColumnHashtable { get; }
        //public Column[] DefiningColumns { get; private set; }
        //public bool[][] Cells { get; private set; }

        //public void SetColumn(Column col, int columnNum)
        //{
        //    if (columnNum < 0)
        //    {
        //        throw new ArgumentException("Cannot set a negative column");
        //    }
        //    if (columnNum >= (Size + 1) / 2)
        //    {
        //        throw new ArgumentException("Cannot set a column past the midpoint");
        //    }
        //    if (columnNum == (Size - 1) / 2 && !col.IsSymetric)
        //    {
        //        throw new ArgumentException("Mid point must be symetric");
        //    }

        //    DefiningColumns[columnNum] = col;

        //    for (int i = 0; i < Size; i++)
        //    {
        //        Cells[i][columnNum] = col.Cells[i];

        //        //If not the midpoint
        //        if (columnNum < (Size - 1) / 2)
        //        {
        //            Cells[Size - 1 - i][Size - 1 - columnNum] = col.Cells[i];
        //        }
        //    }
        //}



        //public bool IsValid()
        //{
        //    for (int i = 0; i < Size; i++)
        //    {
        //        if (!ColumnHashtable.Contains(RowString(i)))
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        //public string ColumnString(int col)
        //{
        //    var cells = new bool[Size];
        //    for (int i = 0; i < Size; i++)
        //    {
        //        cells[i] = Cells[i][col];
        //    }
        //    return CellsToString(cells);
        //}

        //public string RowString(int row)
        //{
        //    return CellsToString(Cells[row].ToArray());
        //}

        //public string CellsToString(bool[] cells)
        //{
        //    string s = "";
        //    foreach (var cell in cells)
        //    {
        //        s += cell ? "1" : "0";
        //    }
        //    return s;
        //}

        //public override string ToString()
        //{
        //    string s = "";
        //    foreach (var col in DefiningColumns)
        //    {
        //        s += s.Length == 0 ? "" : ".";
        //        s += col.ToString();
        //    }
        //    return s;
        //}

        //public int Order =>  DefiningColumns.Sum(x => x.Order);

        public static Hashtable ColumnHashtable { get; set; } = new Hashtable();
        public static int InvalidRow { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderPuzzle"></param>
        /// <param name="candidate">Ref because we calculate word count in this </param>
        /// <returns></returns>
        public static bool IsValidNewPuzzle(Puzzle candidate)
        {
            var cols = GetCols(candidate);
            var rows = GetRows(cols);

            foreach (var row in rows)
            {
                if (!ColumnHashtable.ContainsKey(row))
                {
                    InvalidRow++;
                    return false;
                }
            }

            return true;
        }

        public static string[] GetCols(Puzzle puzzle)
        {
            var cols = new string[puzzle.Size];

            for (int i = 0; i < puzzle.Columns.Length; i++)
            {
                if (i < puzzle.Columns.Length - 1)
                {
                    cols[i] = puzzle.Columns[i];
                    cols[puzzle.Size - 1 - i] = Reverse(puzzle.Columns[i]);
                }
                else
                {
                    cols[i] = ((Column)ColumnHashtable[puzzle.Columns[i]]).ToStringSymetric();
                }
            }

            return cols;
        }

        public static string[] GetRows(string[] cols)
        {
            var rows = new string[cols.Length];
            for (int i = 0; i < cols.Length; i++)
            {
                for (int j = 0; j < cols.Length; j++)
                {
                    rows[i] += cols[j][i];
                }
            }

            return rows;
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static void Print(Puzzle candidate)
        {
            var cols = GetCols(candidate);
            int size = candidate.Size;
            Console.WriteLine(" ");
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write(" " + new string(' ', size) + " ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine($"Order: {candidate.Order}");
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Blue;
            for (int i = 0; i < size; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(" ");
                for (int j = 0; j < size; j++)
                {
                    if (cols[i][j] == '1')
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write("A");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(" ");
                    }
                }

                Console.BackgroundColor = ConsoleColor.Gray;
                //if (ColumnHashtable.Contains(RowString(i)))
                //{
                Console.WriteLine(" ");
                //}
                //else
                //{
                //    Console.WriteLine("  <= Bad");
                //}

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write(" " + new string(' ', size) + " ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" ");

        }
    }
}
