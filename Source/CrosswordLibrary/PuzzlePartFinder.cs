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
                    }
                }
                //else
                //{
                //    if (depth < width - 1)
                //    {
                //        Console.WriteLine($"Invalid - {string.Join(",", newPart)}");         //Stopping search early              
                //    }
                //}
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
