using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordLibrary
{
    public class Column
    {
        public Column(bool[] cells)
        {
            Cells = cells ?? throw new ArgumentNullException(nameof(cells));
        }

        /// <summary>
        /// Boolean array of cells. Letters = true, Black = false
        /// </summary>
        public bool[] Cells { get; private set; }
        public string Parent { get; set; }
        public List<string> Children { get; set; } = new List<string>();


        public bool IsValid()
        {
            if (Cells.Where(x => x).ToArray().Count() < 3)
            {
                return false;
            }

            int wordStart = -1;
            bool inWord = false;
            int i = 0;
            for (i = 0; i < Cells.Length; i++)
            {
                if (inWord)
                {
                    if (!Cells[i])
                    {
                        //Word too short
                        if (i - wordStart < 3)
                        {
                            return false;
                        }

                        inWord = false;
                        wordStart = -1;
                    }
                }
                else
                {
                    if (Cells[i])
                    {
                        inWord = true;
                        wordStart = i;
                    }
                }
            }

            //Word too short
            if (i - wordStart < 3)
            {
                return false;
            }

            return true;
        }

        public int WordCount()
        {
            int wordCount = 0;
            bool lastCell = false;
            for (int i = 0; i < Cells.Length; i++)
            {
                if (lastCell == false && Cells[i] == true) { wordCount++; }
                lastCell = Cells[i];
            }

            return wordCount;
        }

        public int MaxBlackCellSequence()
        {
            int MaxCount = 0;
            int BlackStart = -1;
            bool InBlack = false;
            for (int i = 0; i < Cells.Length; i++)
            {
                if (InBlack == false && Cells[i] == false)
                {
                    InBlack = true;
                    BlackStart = i;
                }
                else if (InBlack == true && Cells[i] == true)
                {
                    InBlack = false;
                    MaxCount = Math.Max(MaxCount, i - BlackStart );
                }
            }

            if (InBlack == true)
            {
                MaxCount = Math.Max(MaxCount, Cells.Count() - BlackStart);
            }

            return MaxCount;
        }

        public void Print(bool print)
        {
            if (!print)
            {
                return;
            }

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(" ");
            foreach (var cell in Cells)
            {
                Console.BackgroundColor = cell ? ConsoleColor.White : ConsoleColor.Black;
                if (cell)
                {
                    Console.Write("A");
                }
                else
                {
                    Console.Write(" ");
                }
            }
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{ToString()} {Order.ToString("00")} " +
                             $"{Parent ?? new string(' ', Cells.Length)} " +
                             $"{(ChangedCell() == -1 ? "  " : ChangedCell().ToString("00"))} " +
                             $"{(Children.Count > 0 ? Children.Count.ToString("00") : "  ")} " +
                             $" {IsSymetric.ToString().PadLeft(5)}" +
                             $" {NoBlackEdges.ToString().PadLeft(5)}" +
                             $" {MaxBlackCellSequence().ToString().PadLeft(2)}" +
                             $" {ValidLeftColumn.ToString().PadLeft(5)}" +
                             $" {WordCount()}" 
                             );

        }

        public int Order => Cells.Where(x => !x).Count();

        /// <summary>
        /// String from cells. Letters = 1, Black = 0
        /// </summary>
        public override string ToString()
        {
            string s = "";
            foreach (var cell in Cells)
            {
                s += cell ? "1" : "0";
            }
            return s;
        }

        public bool IsSymetric => ToString() == new String(ToString().Reverse().ToArray());

        public bool NoBlackEdges => Cells[0] == true && Cells.Last() == true;

        public bool ValidLeftColumn => NoBlackEdges && MaxBlackCellSequence() <= 1;

        public int ChangedCell()
        {
            if (Parent == null)
            {
                return -1;
            }

            return ToString().Zip(Parent, (c1, c2) => c1 == c2).TakeWhile(b => b).Count();
        }
    }
}
