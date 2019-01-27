using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordLibrary
{
    public class Puzzle
    {
        public Puzzle(int size, Hashtable columnHashtable)
        {
            Size = size;
            ColumnHashtable = columnHashtable;
            DefiningColumns = new Column[((size + 1) / 2)];

            Cells = new bool[Size][];

            for (int i = 0; i < Size; i++)
            {
                Cells[i] = new bool[Size];
                for (int j = 0; j < Size; j++)
                {
                    Cells[i][j] = true;
                }
            }
        }

        public int Size { get; private set; }
        public Hashtable ColumnHashtable { get; }
        public Column[] DefiningColumns { get; private set; }
        public bool[][] Cells { get; private set; }

        public void SetColumn(Column col, int columnNum)
        {
            if (columnNum < 0)
            {
                throw new ArgumentException("Cannot set a negative column");
            }
            if (columnNum >= (Size + 1) / 2)
            {
                throw new ArgumentException("Cannot set a column past the midpoint");
            }
            if (columnNum == (Size - 1) / 2 && !col.IsSymetric)
            {
                throw new ArgumentException("Mid point must be symetric");
            }

            DefiningColumns[columnNum] = col;

            for (int i = 0; i < Size; i++)
            {
                Cells[i][columnNum] = col.Cells[i];

                //If not the midpoint
                if (columnNum < (Size - 1) / 2)
                {
                    Cells[Size - 1 - i][Size - 1 - columnNum] = col.Cells[i];
                }
            }
        }

        public void Print()
        {
            Console.WriteLine(" ");
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(" " + new string(' ', Size) + " ");
            for (int i = 0; i < Size; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(" ");
                for (int j = 0; j < Size; j++)
                {
                    Console.BackgroundColor = Cells[i][j] ? ConsoleColor.White : ConsoleColor.Black;
                    if (Cells[i][j])
                    {
                        Console.Write("A");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }

                Console.BackgroundColor = ConsoleColor.Gray;
                if (ColumnHashtable.Contains(RowString(i)))
                {
                    Console.WriteLine(" ");
                }
                else
                {
                    Console.WriteLine("  <= Bad");
                }

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write(" " + new string(' ', Size) + " ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
        }

        public bool IsValid()
        {
            for (int i = 0; i < Size; i++)
            {
                if (!ColumnHashtable.Contains(RowString(i)))
                {
                    return false;
                }
            }
            return true;
        }

        public string ColumnString(int col)
        {
            var cells = new bool[Size];
            for (int i = 0; i < Size; i++)
            {
                cells[i] = Cells[i][col];
            }
            return CellsToString(cells);
        }

        public string RowString(int row)
        {
            return CellsToString(Cells[row].ToArray());
        }

        public string CellsToString(bool[] cells)
        {
            string s = "";
            foreach (var cell in cells)
            {
                s += cell ? "1" : "0";
            }
            return s;
        }

        public override string ToString()
        {
            string s = "";
            foreach (var col in DefiningColumns)
            {
                s += s.Length == 0 ? "" : ".";
                s += col.ToString();
            }
            return s;
        }

        public int Order =>  DefiningColumns.Sum(x => x.Order);
    }
}
