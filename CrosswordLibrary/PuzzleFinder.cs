using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CrosswordLibrary
{
    public class PuzzleFinder
    {
        public ColumnSet ValidColumns { get; private set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public int SearchLimit { get; }
        public int PuzzleSize { get; private set; }
        public bool SearchComplete { get; set; } = true;
        public Hashtable ColumnHashtable { get; set; } = new Hashtable();
        public Hashtable PuzzleHashtable { get; set; } = new Hashtable();
        public int PuzzelsTested { get; set; }
        public int DuplicatesFound { get; set; }
        public int InvalidFound { get; set; }

        public PuzzleFinder(ColumnSet validColumns, int searchLimit, bool print = false)
        {

            ValidColumns = validColumns;
            SearchLimit = searchLimit;
            foreach (var item in validColumns.Columns)
            {
                ColumnHashtable.Add(item.ToString(), item);
            }
            PuzzleSize = validColumns.ColumnSize;

            if (PuzzleSize % 2 == 0)
            {
                throw new ArgumentException("Puzzle size may not be even");
            }

            //ValidColumns.PrintInfo();
            //Console.WriteLine("");


            Search();

            Console.WriteLine($"Found {PuzzleHashtable.Count} puzzles. Size {PuzzleSize}. Checked puzzels {PuzzelsTested}. SearchLimit {SearchLimit}. Elapsed {(DateTime.Now - StartTime).TotalSeconds} seconds");
            //PrintPuzzels(print);
        }

        private void PrintPuzzels(bool print)
        {
            var puzzelList = new List<Puzzle>();
            int i = 0;
            if (print)
            {
                var puzzles = PuzzleHashtable.Values;
                foreach (Puzzle item in puzzles)
                {
                    puzzelList.Add(item);
                }

                Console.ReadKey();
                //foreach (var item in puzzelList.OrderByDescending(x => x.ToString()))
                var orderedList = puzzelList.OrderByDescending(x => x.Order);
                foreach (var item in orderedList)
                {

                    item.Print();
                    i++;
                    if (i % 10 == 0)
                    {
                        Console.ReadKey();
                    }
                }
            }
        }

        private void Search()
        {
            List<Puzzle> currentValidPuzzels = new List<Puzzle>();
            List<Puzzle> newValidPuzzels = new List<Puzzle>();

            var rootTable = GetRootTable();

            if (rootTable.IsValid())
            {
                //PuzzleHashtable.Add(rootTable.ToString(), rootTable);
                PuzzleHashtable.Add(rootTable.ToString(), rootTable.ToString());
                currentValidPuzzels.Add(rootTable);
            }

            Console.WriteLine("");
            //Console.WriteLine($"Searching {SearchLimit} times");
            for (int i = 0; i < SearchLimit; i++)
            {
                Console.WriteLine($"Search Loop {i} - {currentValidPuzzels.Count} Puzzles");
                foreach (var parentPuzzel in currentValidPuzzels)
                {
                    //Console.WriteLine($"Checking {parentPuzzel}");

                    List<Puzzle> childPuzzels = FindValidChildPuzzles(parentPuzzel, parentPuzzel.DefiningColumns.Length, 0, i);
                    foreach (var child in childPuzzels)
                    {
                        if (!PuzzleHashtable.Contains(child.ToString()))
                        {
                            //PuzzleHashtable.Add(child.ToString(), child);
                            PuzzleHashtable.Add(child.ToString(), child.ToString());
                            newValidPuzzels.Add(child);
                        }
                        else
                        {
                            DuplicatesFound++;
                        }
                    }
                }

                Console.WriteLine($"Found {PuzzleHashtable.Count.ToString().PadLeft(10, ' ')} puzzles. Checked puzzels {PuzzelsTested}. Invalid puzzels {InvalidFound}. Duplicate {DuplicatesFound}. Size {PuzzleSize}. Search Depth {i}. {(DateTime.Now - StartTime).TotalSeconds} seconds");

                currentValidPuzzels = newValidPuzzels;
                newValidPuzzels = new List<Puzzle>();
            }
        }

        private List<Puzzle> FindValidChildPuzzles(Puzzle parentPuzzel, int totalCols, int thisCol, int depth)
        {
            //Console.WriteLine($"GetChildren for puzzle {parentPuzzel} Col {thisCol}.");
            var validChildren = new List<Puzzle>();

            if (parentPuzzel.DefiningColumns[thisCol].Order != depth)
            {
                //Its the wrong loop to check this
                return validChildren;
            }

            List<Column> childCols = GetChildren(parentPuzzel, thisCol, totalCols == thisCol + 1);
            if (childCols.Count == 0)
            {
                return validChildren;
            }

            for (int i = 0; i <= childCols.Count; i++)
            {
                var newPuzzle = CopyPuzzle(parentPuzzel);
                PuzzelsTested++;
                //On the last loop, we don't alter the column
                if (i > 0)
                {
                    //Console.WriteLine($"Checking col {thisCol}");
                    newPuzzle.SetColumn(childCols[i - 1], thisCol);
                }
                else
                {
                    //Do nothing
                    //Console.Write("");
                }
                if (!newPuzzle.IsValid())
                {
                    //Console.Write(" <== Invalid");
                    InvalidFound++;
                    break;
                }


                if (!PuzzleHashtable.Contains(newPuzzle.ToString()))
                {
                    validChildren.Add(newPuzzle);
                }
                if (thisCol + 1 < totalCols)
                {
                    validChildren.AddRange(FindValidChildPuzzles(newPuzzle, totalCols, thisCol + 1, depth));
                }
            }

            return validChildren;
        }

        private Puzzle CopyPuzzle(Puzzle old)
        {
            var puzzle = new Puzzle(old.Size, old.ColumnHashtable);
            for (int i = 0; i < old.DefiningColumns.Length; i++)
            {
                puzzle.SetColumn(old.DefiningColumns[i], i);
            }
            return puzzle;
        }

        private List<Column> GetChildren(Puzzle parentPuzzel, int thisCol, bool median)
        {
            //Console.WriteLine($"GetChildren for Col {thisCol}. Median {median}");

            //if Not Median column
            var childCols = new List<Column>();
            var childStrings = parentPuzzel.DefiningColumns[thisCol].Children;
            foreach (var childString in childStrings)
            {
                var col = (Column)ColumnHashtable[childString];
                if (col.IsSymetric || !median)
                {
                    childCols.Add(col);
                }
            }

            return childCols;
        }

        private Puzzle GetRootTable()
        {
            var rootColumn = ValidColumns.GetColumnsByOrder(0).First();
            var rootPuzzle = new Puzzle(PuzzleSize, ColumnHashtable);

            for (int i = 0; i < rootPuzzle.DefiningColumns.Length; i++)
            {
                rootPuzzle.SetColumn(rootColumn, i);
            }

            return rootPuzzle;
        }
    }
}
