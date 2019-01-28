using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordLibrary
{
    public class PuzzleChecker
    {
        public Hashtable ColumnHashtable { get; set; } = new Hashtable();
        public long InvalidRowCount { get; set; } = 0;
        public long CheaterCount { get; set; } = 0;
        public long NotContiuousCount { get; set; } = 0;
        public int Size { get; set; } = 0;
        public int Mid { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderPuzzle"></param>
        /// <param name="candidate">Ref because we calculate word count in this </param>
        /// <returns></returns>
        public bool IsValidNewPuzzle(Puzzle candidate)
        {
            var cols = GetCols(candidate);

            for (int i = 0; i < candidate.Size; i++)
            {
                var row = GetRow(cols, i);
                var col = (Column)ColumnHashtable[row];
                if (col == null)
                {
                    //Catch when not a valid key
                    InvalidRowCount++;
                    return false;
                }

                //Test the special case that the three top and three bottom rows MUST be eligible to be in the left colum, or they are cheaters.
                if ((i <= 2 || i >= candidate.Size - 3) && col.ValidLeftColumn == false)
                {
                    InvalidRowCount++;
                    return false;
                }

            }

            if (HasCheater(cols))
            {
                CheaterCount++;
                return false;
            }

            if (NotContinous(cols))
            {
                NotContiuousCount++;
                return false;
            }

            return true;
        }

        private bool NotContinous(string[] cols)
        {
            return false;
        }

        public string[] GetCols(Puzzle puzzle)
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

        public string GetRow(string[] cols, int index)
        {
            var sb = new StringBuilder(Size);
            for (int i = 0; i < cols.Length; i++)
            {
                sb.Append(cols[i][index]);
            }

            return sb.ToString();
        }

        public string[] GetRows(string[] cols)
        {

            var rows = new string[cols.Length];
            var sb = new StringBuilder(Size);
            for (int i = 0; i < cols.Length; i++)
            {
                for (int j = 0; j < cols.Length; j++)
                {
                    sb.Append(cols[j][i]);
                }
                rows[i] = sb.ToString();
                sb.Clear();
            }

            return rows;
        }

        public bool HasCheater(string[] cols)
        {
            for (int i = 0; i < Mid; i++)
            {
                for (int j = 0; j < cols[0].Length; j++)
                {
                    if (cols[i][j] == '1')
                    {
                        continue; //this is a white cell, can't be a cheat
                    }

                    //Try all four types of 'L' cheaters. 
                    if (i > 0)
                    {
                        if (j > 0)
                        {
                            if (cols[i - 1][j] == '0' && cols[i][j - 1] == '0') { return true; }
                        }

                        if (j < Size - 1)
                        {
                            if (cols[i - 1][j] == '0' && cols[i][j + 1] == '0') { return true; }
                        }
                    }

                    if (i < Size - 1)
                    {
                        if (j > 0)
                        {
                            if (cols[i + 1][j] == '0' && cols[i][j - 1] == '0') { return true; }
                        }

                        if (j < Size - 1)
                        {
                            if (cols[i + 1][j] == '0' && cols[i][j + 1] == '0') { return true; }
                        }
                    }
                }
            }

            return false;
        }

        public string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public void Print(Puzzle candidate)
        {
            var cols = GetCols(candidate);
            int size = candidate.Size;
            Console.WriteLine(" ");
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write(" " + new string(' ', size) + " ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine($" Order: {candidate.Order}");
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Blue;
            for (int i = 0; i < size; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(" ");
                for (int j = 0; j < size; j++)
                {
                    if (cols[j][i] == '1')
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
                Console.WriteLine(" ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write(" " + new string(' ', size) + " ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
            Console.WriteLine(" ");

        }
    }
}
