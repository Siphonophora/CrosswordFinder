using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace CrosswordLibrary
{
    public class PuzzlePartFinder
    {
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



        public List<string[]> FindParts(ColumnGroup validColumns, Enums.PuzzlePart partType, IColumnPicker columnPicker, int size)
        {
            var puzzleChecker = new PuzzleChecker(size);
            var partWidth = partType == Enums.PuzzlePart.Left ? puzzleChecker.LeftPartSize : puzzleChecker.CentPartSize;
            var rootPart = GetRootPart(validColumns, partWidth);

            var foundParts = FindPartsRecursive(puzzleChecker, validColumns, partType, columnPicker, rootPart, partWidth, 0);

            if (partType == Enums.PuzzlePart.Center)
            {
                foundParts = ReverseParts(foundParts);
            }

            return foundParts;
        }

        private List<string[]> ReverseParts(List<string[]> foundParts)
        {
            return foundParts.Select(x => x.Reverse().ToArray()).ToList();
        }

        private List<string[]> FindPartsRecursive(PuzzleChecker puzzleChecker, ColumnGroup validColumns, Enums.PuzzlePart partType, IColumnPicker columnPicker, string[] basePart, int width, int depth)
        {
            var parts = new List<string[]>();
            var options = columnPicker.Pick(validColumns, basePart, depth);

            for (int i = 0; i < options.Count; i++)
            {
                var newPart = basePart.ToArray();
                newPart[depth] = options[i];

                if (PartValid(puzzleChecker, partType, newPart))
                {
                    if (depth < width - 1)
                    {
                        parts.AddRange(FindPartsRecursive(puzzleChecker, validColumns, partType, columnPicker, newPart, width, depth + 1));
                    }
                    else
                    {
                        parts.Add(newPart);
                        TotalValidCount++;
                        //Console.WriteLine($"Valid {TotalValidCount} {string.Join(", ", newPart.Take(depth + 1))}");

                        
                    }
                }
                else
                {
                    if (depth < width - 1)
                    {
                        SlicesSkipped++;
                        Console.WriteLine($"Skipp further checks {string.Join(", ", newPart.Take(depth + 1))}");
                    }
                    else //If we are at full depth
                    {
                        //Console.WriteLine("Invalid");
                    }
                                      
                }
            }

            return parts;
        }

        /// <summary>
        /// Parts can only be invalid if their partial rows are invalid. We already prevent cheaters from being neighbors so that is OK. 
        /// </summary>
        /// <param name="puzzleChecker"></param>
        /// <param name="partType"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        private bool PartValid(PuzzleChecker puzzleChecker, Enums.PuzzlePart partType, string[] part)
        {
            bool left = partType == Enums.PuzzlePart.Left;
            if (puzzleChecker.PartialRowsAreValid(part, left) == false)
            {
                return false;
            }

            return true;
        }

        private void WriteStatus()
        {
            int pad = 30;

            Console.WriteLine("------------------------------Status--------------------------------------------");
            Console.WriteLine($" Elapsed Seconds       {(DateTime.Now - StartTime).TotalSeconds.ToString("0.0").PadLeft(pad, ' ')}");
            Console.WriteLine($" Approximate Progress  {((double)(SlicesChecked + SliceChildrenSkipped) / TotalSliceCount).ToString("P").PadLeft(pad, ' ')}");
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

        private void LogStatus()
        {
            Log.Logger.Information(" Elapsed Seconds {sec}" +
           " Approximate Progress {prog}" +
           " Slice Info {sliceinfo}" +
           " Total Slices {totalslices}" +
           " Checked Slices {checkedslices}" +
           " Skipped Slices {skippedslices}" +
           " Puzzels Checked {puzzelschecked}" +
           " Total Invalid Row: {totalinvalidrows}" +
           " Total Cheater: {totalcheater}" +
           " Total Not Continuous: {totalnotcontinuous}" +
           " Total Valid: {totalvalid}"

            , (DateTime.Now - StartTime).TotalSeconds
            , (double)(SlicesChecked + SliceChildrenSkipped) / TotalSliceCount
            , SliceInfo.Trim()
            , TotalSliceCount
            , SlicesChecked
            , SliceChildrenSkipped
            , NumChecked
            , PuzzleChecker.InvalidRowCount
            , PuzzleChecker.CheaterCount
            , PuzzleChecker.NotContiuousCount
            , TotalValidCount
            );

        }


        private string[] GetRootPart(ColumnGroup columns, int width)
        {
            var rootColumn = columns.Columns.Where(x => x.Order == 0).First().ToString();

            var part = new string[width];

            for (int i = 0; i < width; i++)
            {
                part[i] = rootColumn.ToString();
            }

            return part;
        }
    }
}
