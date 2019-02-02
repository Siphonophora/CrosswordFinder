using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordLibrary
{
    public class ColumnClassifier
    {
        public ColumnClassifier(ColumnGroup validColumns)
        {
            ValidColumns = validColumns ?? throw new ArgumentNullException(nameof(validColumns));

            Analyze();
            PrintClassifications();
        }

        public ColumnGroup ValidColumns { get; set; }

        private void Analyze()
        {
            
            //Find Neighbors
            var puzzleChecker = new PuzzleChecker(ValidColumns.Columns[0].Cells.Length);
            for (int i = 0; i < ValidColumns.Columns.Count; i++)
            {
                var thisCol = ValidColumns.Columns[i];
                var selfpair = new string[] { thisCol.ToString(), thisCol.ToString() };

                //Many columns can't be neighbors with themselves. 
                if (puzzleChecker.IsCheater(selfpair) == false) 
                {
                    thisCol.Neighbors.Add(thisCol.ToString());
                }

                for (int j = i + 1; j < ValidColumns.Columns.Count; j++)
                {
                    var chckCol = ValidColumns.Columns[j];
                    var pair = new string[] { thisCol.ToString(), chckCol.ToString() };
                    if (puzzleChecker.IsCheater(pair) == false)
                    {
                        chckCol.Neighbors.Add(thisCol.ToString());
                        thisCol.Neighbors.Add(chckCol.ToString());
                    }

                    //Check subsequent
                    if(thisCol.ToString() == chckCol.ToStringReverse())
                    {
                        thisCol.HasFlippedTwin = true;
                        chckCol.HasFlippedTwin = true;

                        thisCol.IsPrimaryTwin = true;
                    }
                }
            }
        }

        private bool ValidLink(string parent, string child)
        {
            int changes = 0;
            for (int i = 0; i < parent.Length; i++)
            {
                if (parent[i] == '0' && child[i] == '1')
                {
                    return false;
                }
                if (parent[i] == '1' && child[i] == '0')
                {
                    changes++;
                    if (changes > 1)
                    {
                        return false;
                    }
                }
            }

            return changes == 1;
        }


        private void PrintClassifications()
        {
            foreach (var column in ValidColumns.Columns.OrderBy(x => x.Order))
            {
                column.Print(true);
            }

            ValidColumns.PrintInfo();

            Console.WriteLine();
            Console.WriteLine($"Total Columns: {ValidColumns.Columns.Count}");
            Console.WriteLine($"Center Columns: {ValidColumns.Columns.Where(x => x.IsSymetric).Count()}");
            Console.WriteLine($"ValidLeftColumn Columns: {ValidColumns.Columns.Where(x => x.ValidLeftColumn).Count()}");
            Console.WriteLine($"Avg Neighbors: {ValidColumns.Columns.Average(x => x.Neighbors.Count).ToString("0")}");
            Console.WriteLine();
        }
    }
}
