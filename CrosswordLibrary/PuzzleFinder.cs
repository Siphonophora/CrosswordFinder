using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        public long SlicesSkipped { get; set; }
        public long SliceChildrenSkipped { get; set; }
        public List<Puzzle> ValidPuzzels { get; set; } = new List<Puzzle>();
        public long NumChecked { get; set; } = 0;
        public PuzzleChecker PuzzleChecker { get; set; }
        public string SliceInfo { get; set; } = "";
        public long SlicesChecked { get; private set; }
        public long TotalSliceCount { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="validColumns"></param>
        /// <param name="searchLimit"></param>
        /// <param name="print"></param>
        /// <param name="numOfSlices">This is how many columns deep we will 'slice' the solving into</param>
        public PuzzleFinder(ColumnSet validColumns, int searchLimit, bool print = false, int numOfSlices = 0)
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
            if (numOfSlices == 0)
            {
                Search(searchLimit, 0, null);
            }
            else
            {
                var options = GetChildCandidateOptions(GetRootTable(), 0);
                SearchSlices(searchLimit, options, numOfSlices);
            }

            Console.WriteLine("FINAL!!!!!!!!");
            WriteStatus();

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

        private void SearchSlices(int searchLimit, string[][] options, int numOfSlices, int[] slices = null, int depth = 0)
        {
            if(depth == 0)
            {
                TotalSliceCount = Combinator.SliceChildCount(options);
            }

            for (int i = 0; i < options[depth].Length; i++)
            {
                var newSlices = new int[depth + 1];
                if (slices != null)
                {
                    for (int j = 0; j < slices.Length; j++)
                    {
                        newSlices[j] = slices[j];
                    }
                }

                newSlices[depth] = i;
                SliceInfo = $"{string.Join(",", newSlices)}";

                var baseSlice = Combinator.FindSliceStart(GetRootTable().Columns, options, newSlices);
                if (BaseSliceValid(baseSlice))
                {
                    if (numOfSlices > depth + 1)
                    {
                        SearchSlices(searchLimit, options, numOfSlices, newSlices, depth + 1);
                    }
                    else
                    {
                        ValidPuzzels = new List<Puzzle>();
                        ValidPuzzels.Add(GetRootTable());
                        SlicesChecked++;
                        Search(searchLimit, 0, newSlices);
                    }
                }
                else
                {
                    int numSkipped = Combinator.SliceChildCount(options, newSlices);

                    SlicesSkipped++;
                    SliceChildrenSkipped += numSkipped;

                    //Console.BackgroundColor = ConsoleColor.Green;
                    //Console.WriteLine($" **** Skipped {SliceInfo} - Had {numSkipped} children");
                    //Console.BackgroundColor = ConsoleColor.Black;
                }
            }
        }

        private bool BaseSliceValid(string[] baseSlice)
        {
            var basePuzzle = new Puzzle(PuzzleSize);
            basePuzzle.Columns = baseSlice;

            bool valid = PuzzleChecker.IsValidNewPuzzle(basePuzzle);

            //if (!valid)
            //{
            //    Console.BackgroundColor = ConsoleColor.Green;
            //    Console.WriteLine("Skipping slice based on:");
            //    Console.BackgroundColor = ConsoleColor.Black;
            //    PuzzleChecker.Print(basePuzzle);
            //}

            return valid;
        }

        private void WriteStatus()
        {
            int pad = 30;

            Console.WriteLine("------------------------------Status--------------------------------------------");
            Console.WriteLine($" Elapsed Seconds       {(DateTime.Now - StartTime).TotalSeconds.ToString("0.0").PadLeft(pad,' ')}");
            Console.WriteLine($" Approximate Progress  {((SlicesChecked + SliceChildrenSkipped)/TotalSliceCount).ToString("P").PadLeft(pad, ' ')}");
            Console.WriteLine($" Slice Info            {SliceInfo.PadLeft(pad, ' ')} ");
            Console.WriteLine($" Total Slices          {TotalSliceCount.ToString("N0").PadLeft(pad, ' ')} ");
            Console.WriteLine($" Checked Slices        {SlicesChecked.ToString("N0").PadLeft(pad, ' ')} ");
            Console.WriteLine($" Skipped Slices        {SliceChildrenSkipped.ToString("N0").PadLeft(pad, ' ')} ");
            Console.WriteLine($" Puzzels Checked       {NumChecked.ToString("N0").PadLeft(pad, ' ')}");
            Console.WriteLine($" Total Invalid Row:    {(PuzzleChecker.InvalidRowCount).ToString("N0").PadLeft(pad, ' ')} ");
            Console.WriteLine($" Total Cheater:        {(PuzzleChecker.CheaterCount).ToString("N0").PadLeft(pad, ' ')} ");
            Console.WriteLine($" Total Not Continuous: {(PuzzleChecker.NotContiuousCount).ToString("N0").PadLeft(pad, ' ')} ");
            Console.WriteLine($" Total Valid:          {(TotalValidCount).ToString("N0").PadLeft(pad, ' ')}");
            Console.WriteLine("---------------------------------------------------------------------------------");
                                           
        }

        private void Search(int limit, int order, int[] slices)
        {

            long startPuzzleCount = TotalValidCount;
            long startInvalidRow = PuzzleChecker.InvalidRowCount;
            long startCheater = PuzzleChecker.CheaterCount;
            long startNotContinous = PuzzleChecker.NotContiuousCount;
            var orderPuzzles = ValidPuzzels.Where(x => x.Order == order).ToList();
            

            if (orderPuzzles.Count == 0)
            {
                //TODO Log slice final results;

                return;
            }

            ValidPuzzels.RemoveAll(x => x.Order != order); //Memory managemenet

            for (int i = 0; i < orderPuzzles.Count; i++)
            {
                var options = GetChildCandidateOptions(orderPuzzles[i], order);

                List<Puzzle> childCandidates = new List<Puzzle>();

                var combinations = Combinator.FindCombinations(orderPuzzles[i].Columns, options, 0, true, slices);

                if (combinations.Count > 0)
                {
                    foreach (var combo in combinations)
                    {
                        var newPuzzle = orderPuzzles[i];
                        newPuzzle.Columns = combo;
                        newPuzzle.Order++;
                        childCandidates.Add(newPuzzle);
                    }

                }

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
                        WriteStatus();
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

                //Console.WriteLine(
                //    $"{SliceInfo}" +
                //    $" Depth :{(order + 1).ToString("00")} of {SearchLimit}. " +
                //    $" {NumChecked.ToString("N0").PadLeft(pad, ' ')} checked. " +
                //    $"Elapsed: {(DateTime.Now - StartTime).TotalSeconds.ToString("0.0").PadLeft(pad, ' ')} seconds" +
                //    $" Total Valid: {(TotalValidCount).ToString("N0").PadLeft(pad, ' ')} puzzles of" +
                //    $" New Valid: {(TotalValidCount - startPuzzleCount).ToString("N0").PadLeft(pad, ' ')} puzzles of" +
                //    $" Invalid Row: {(PuzzleChecker.InvalidRowCount - startInvalidRow).ToString("N0").PadLeft(pad, ' ')} " +
                //    $" Cheater: {(PuzzleChecker.CheaterCount - startCheater).ToString("N0").PadLeft(pad, ' ')} " +
                //    $" Not Continuous: {(PuzzleChecker.NotContiuousCount - startNotContinous).ToString("N0").PadLeft(pad, ' ')} "
                //    );

                Search(limit, order + 1, null);
            }
        }

        private string[][] GetChildCandidateOptions(Puzzle orderPuzzle, int order)
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

            return options;

        }



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
