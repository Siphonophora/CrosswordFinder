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
        public long PuzzelsTested { get; set; }
        public long DuplicatesFound { get; set; }
        public long InvalidFound { get; set; }
        public long TotalValidCount { get; set; }
        public List<Puzzle> ValidPuzzels { get; set; } = new List<Puzzle>();
        public long NumChecked { get; set; } = 0;
        public PuzzleChecker PuzzleChecker { get; set; }

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

            PuzzleChecker = new PuzzleChecker
            {
                ColumnHashtable = ColumnHashtable,
                Size = PuzzleSize,
                Mid = (PuzzleSize + 1) / 2
            };

            //ValidColumns.PrintInfo();
            //Console.WriteLine("");


            ValidPuzzels.Add(GetRootTable());
            Search(searchLimit);

            Console.WriteLine();
            Console.WriteLine($"Found {ValidPuzzels.Count} puzzles. " +
                $"Size {PuzzleSize}. " +
                $"Checked puzzels {PuzzelsTested}. " +
                $"SearchLimit {SearchLimit}. " +
                $"Elapsed {(DateTime.Now - StartTime).TotalSeconds} seconds");
            Console.WriteLine();

            int printPause = 0;
            int printSkip = 10;
            if (print)
            {
                for (int i = 0; i < ValidPuzzels.Count; i++)
                {
                    if (i % printSkip == 0)
                    {
                        if (printPause % 5 == 0) { Console.ReadKey(); }
                        PuzzleChecker.Print(ValidPuzzels[i]);
                        printPause++;

                    }
                }
            }
        }


        private void Search(int limit, int order = 0)
        {

            long startPuzzleCount = TotalValidCount;
            long startInvalidRow = PuzzleChecker.InvalidRowCount;
            long startCheater = PuzzleChecker.CheaterCount;
            long startNotContinous = PuzzleChecker.NotContiuousCount;
            var orderPuzzles = ValidPuzzels.Where(x => x.Order == order).ToList();
            int pad = 15;

            if (orderPuzzles.Count == 0)
            {
                Console.WriteLine("Done searching");
                return;
            }

            ValidPuzzels.RemoveAll(x => x.Order < order); //Memory managemenet

            for (int i = 0; i < orderPuzzles.Count; i++)
            {
                List<Puzzle> childCandidates = GetChildCandidates(orderPuzzles[i], order);

                for (int j = 0; j < childCandidates.Count; j++)
                {
                    if (PuzzleChecker.IsValidNewPuzzle(childCandidates[j]))
                    {
                        ValidPuzzels.Add(childCandidates[j]);
                        TotalValidCount++;
                    }

                    NumChecked++;
                    if (NumChecked % 1000000 == 0)
                    {
                        Console.WriteLine(
                            $" {NumChecked.ToString("N0").PadLeft(pad, ' ')} checked. " +
                            $"Working on {(j + 1).ToString("N0").PadLeft(pad, ' ')} of {childCandidates.Count.ToString("N0").PadLeft(pad, ' ')} children " +
                            $"of # {(i + 1).ToString("N0").PadLeft(pad, ' ')} of {orderPuzzles.Count.ToString("N0").PadLeft(pad, ' ')} puzzels " +
                            $"for order {order} " +
                            $"Elapsed: {(DateTime.Now - StartTime).TotalSeconds.ToString("0.0").PadLeft(pad, ' ')} seconds");

                        PuzzleChecker.Print(ValidPuzzels.Last());
                    }
                }
            }


            if (limit == order)
            {
                Console.WriteLine("Done searching - HIT SEARCH ORDER LIMIT");
                return;
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(
                    $" {NumChecked.ToString("N0").PadLeft(pad, ' ')} checked. " +
                    $"Elapsed: {(DateTime.Now - StartTime).TotalSeconds.ToString("0.0").PadLeft(pad, ' ')} seconds" +
                    $" Total Valid: {(TotalValidCount).ToString("N0").PadLeft(pad, ' ')} puzzles of" +
                    $" New Valid: {(TotalValidCount - startPuzzleCount).ToString("N0").PadLeft(pad, ' ')} puzzles of" +
                    $" Invalid Row: {(PuzzleChecker.InvalidRowCount - startInvalidRow).ToString("N0").PadLeft(pad, ' ')} " +
                    $" Cheater: {(PuzzleChecker.CheaterCount - startCheater).ToString("N0").PadLeft(pad, ' ')} " +
                    $" Not Continuous: {(PuzzleChecker.NotContiuousCount - startNotContinous).ToString("N0").PadLeft(pad, ' ')} " +
                    $"Depth :{order + 1} of {SearchLimit}. ");
                Console.WriteLine();

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
                            if (ColumnHashtable.ContainsKey(childCol.SymetricString))
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
