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
        public List<Puzzle> ValidPuzzels { get; set; } = new List<Puzzle>();

        public PuzzleFinder(ColumnSet validColumns, int searchLimit, bool print = false)
        {

            ValidColumns = validColumns;
            SearchLimit = searchLimit;
            foreach (var item in validColumns.Columns)
            {
                ColumnHashtable.Add(item.ToString(), item);
            }
            PuzzleChecker.ColumnHashtable = ColumnHashtable;
            PuzzleSize = validColumns.ColumnSize;

            if (PuzzleSize % 2 == 0)
            {
                throw new ArgumentException("Puzzle size may not be even");
            }

            //ValidColumns.PrintInfo();
            //Console.WriteLine("");


            ValidPuzzels.Add(GetRootTable());
            Search(searchLimit);

            Console.WriteLine($"Found {ValidPuzzels.Count} puzzles. Size {PuzzleSize}. Checked puzzels {PuzzelsTested}. SearchLimit {SearchLimit}. Elapsed {(DateTime.Now - StartTime).TotalSeconds} seconds");


            int printPause = 0;
            if (print)
            {
                foreach (var puzzle in ValidPuzzels)
                {
                    if (printPause % 10 == 0) { Console.ReadKey(); }
                    PuzzleChecker.Print(puzzle);
                    printPause++;
                }
            }
        }

        //private void PrintPuzzels(bool print)
        //{
        //    var puzzelList = new List<PuzzleChecker>();
        //    int i = 0;
        //    if (print)
        //    {
        //        var puzzles = PuzzleHashtable.Values;
        //        foreach (PuzzleChecker item in puzzles)
        //        {
        //            puzzelList.Add(item);
        //        }

        //        Console.ReadKey();
        //        //foreach (var item in puzzelList.OrderByDescending(x => x.ToString()))
        //        var orderedList = puzzelList.OrderByDescending(x => x.Order);
        //        foreach (var item in orderedList)
        //        {

        //            item.Print();
        //            i++;
        //            if (i % 10 == 0)
        //            {
        //                Console.ReadKey();
        //            }
        //        }
        //    }
        //}

        private void Search(int limit, int order = 0)
        {

            var orderPuzzles = ValidPuzzels.Where(x => x.Order == order).ToList();

            foreach (var orderPuzzle in orderPuzzles)
            {
                List<Puzzle> childCandidates = GetChildCandidates(orderPuzzle, order);

                foreach (var candidate in childCandidates)
                {
                    if (PuzzleChecker.IsValidNewPuzzle(candidate))
                    {
                        ValidPuzzels.Add(candidate);
                    }
                }
            }


            if (limit == order)
            {
                Console.WriteLine("Done searching");
                return;
            }
            else
            {
                Search(limit, order + 1);
            }
        }

        private List<Puzzle> GetChildCandidates(Puzzle orderPuzzle, int order)
        {
            var results = new List<Puzzle>();
            var options = new string[orderPuzzle.Columns.Length][];

            for (int i = 0; i < orderPuzzle.Columns.Length; i++)
            {
                var currentCol = (Column)ColumnHashtable[orderPuzzle.Columns[i]];

                var possibleCols = new List<string>();
                possibleCols.Add(currentCol.ToString()); //Current column is always an option
                if (currentCol.Order == order) //Only check current order columns
                {

                    var children = currentCol.Children;

                    foreach (var child in children)
                    {
                        var childCol = (Column)ColumnHashtable[child];


                        if ((i <= 2 && childCol.ValidLeftColumn)
                            || (i > 2 && i < orderPuzzle.Columns.Length - 1)
                           )
                        {
                            possibleCols.Add(child);
                        }
                        else if (i == orderPuzzle.Columns.Length - 1 && childCol.CanBeCenterColumn())
                        {
                            //The symetric versio might not be valid so, check that it is
                            if (ColumnHashtable.ContainsKey(childCol.ToStringSymetric()))
                            {
                                possibleCols.Add(child);
                            }
                        }
                    }
                }
                options[i] = possibleCols.ToArray();
            }

            var combinations = Combinator.FindCombinations(orderPuzzle.Columns, options);

            if (combinations.Count == 0)
            {
                return results;
            }

            foreach (var combo in combinations)
            {
                var newPuzzle = orderPuzzle;
                newPuzzle.Columns = combo;
                newPuzzle.Order++;
                results.Add(newPuzzle);
            }

            return results;
        }

        //private List<PuzzleChecker> FindValidChildPuzzles(PuzzleChecker parentPuzzel, int totalCols, int thisCol, int depth)
        //{
        //    //Console.WriteLine($"GetChildren for puzzle {parentPuzzel} Col {thisCol}.");
        //    var validChildren = new List<PuzzleChecker>();

        //    if (parentPuzzel.DefiningColumns[thisCol].Order != depth)
        //    {
        //        //Its the wrong loop to check this
        //        return validChildren;
        //    }

        //    List<Column> childCols = GetChildren(parentPuzzel, thisCol, totalCols == thisCol + 1);
        //    if (childCols.Count == 0)
        //    {
        //        return validChildren;
        //    }

        //    for (int i = 0; i <= childCols.Count; i++)
        //    {
        //        var newPuzzle = CopyPuzzle(parentPuzzel);
        //        PuzzelsTested++;
        //        //On the last loop, we don't alter the column
        //        if (i > 0)
        //        {
        //            //Console.WriteLine($"Checking col {thisCol}");
        //            newPuzzle.SetColumn(childCols[i - 1], thisCol);
        //        }
        //        else
        //        {
        //            //Do nothing
        //            //Console.Write("");
        //        }
        //        if (!newPuzzle.IsValid())
        //        {
        //            //Console.Write(" <== Invalid");
        //            InvalidFound++;
        //            break;
        //        }


        //        if (!PuzzleHashtable.Contains(newPuzzle.ToString()))
        //        {
        //            validChildren.Add(newPuzzle);
        //        }
        //        if (thisCol + 1 < totalCols)
        //        {
        //            validChildren.AddRange(FindValidChildPuzzles(newPuzzle, totalCols, thisCol + 1, depth));
        //        }
        //    }

        //    return validChildren;
        //}

        //private PuzzleChecker CopyPuzzle(PuzzleChecker old)
        //{
        //    var puzzle = new PuzzleChecker(old.Size, old.ColumnHashtable);
        //    for (int i = 0; i < old.DefiningColumns.Length; i++)
        //    {
        //        puzzle.SetColumn(old.DefiningColumns[i], i);
        //    }
        //    return puzzle;
        //}

        //private List<Column> GetChildren(PuzzleChecker parentPuzzel, int thisCol, bool median)
        //{
        //    //Console.WriteLine($"GetChildren for Col {thisCol}. Median {median}");

        //    //if Not Median column
        //    var childCols = new List<Column>();
        //    var childStrings = parentPuzzel.DefiningColumns[thisCol].Children;
        //    foreach (var childString in childStrings)
        //    {
        //        var col = (Column)ColumnHashtable[childString];
        //        if (col.IsSymetric || !median)
        //        {
        //            childCols.Add(col);
        //        }
        //    }

        //    return childCols;
        //}

        private Puzzle GetRootTable()
        {
            var rootColumn = ValidColumns.GetColumnsByOrder(0).First().ToString();

            Puzzle puzzle = new Puzzle(PuzzleSize);

            for (int i = 0; i < puzzle.Columns.Length; i++)
            {
                puzzle.Columns[i] = rootColumn;
            }

            puzzle.Order = 0;
            puzzle.WordCount = PuzzleSize * 2;

            return puzzle;
        }
    }
}
