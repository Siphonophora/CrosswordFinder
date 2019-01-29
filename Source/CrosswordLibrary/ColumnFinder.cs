using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordLibrary
{
    public class ColumnFinder
    {

        public bool Print { get; }
        public bool SearchComplete { get; set; } = true;
        public List<bool[]> ColumnCells { get; set; } = new List<bool[]>();
        public ColumnSet ValidColumns { get; set; } 

        public ColumnFinder(int columnSize, bool print = false)
        {
            ValidColumns = new ColumnSet(columnSize);
            Print = print;
            var cells = new bool[columnSize];
            
            FindValidChildren(cells);

            //Console.WriteLine($"Column {columnSize} cells has {ColumnCells.Count} members.");
        }

        private void FindValidChildren(bool[] cells, int depth = -1)
        {
            depth++;
            if (depth >= ValidColumns.ColumnSize)
            {
                SearchComplete = false;
                return;
            }
            TestOption(cells, depth, true);
            TestOption(cells, depth, false);
        }

        private void TestOption(bool[] cells, int depth, bool cellState)
        {
            var newCells = cells.ToArray();
            newCells[depth] = cellState;

            FindValidChildren(newCells, depth);

            //Until we are at the full search depth, we don't have a valid result
            if (depth == ValidColumns.ColumnSize - 1)
            {
                var newColumn = new Column(newCells);
                if (newColumn.IsValid())
                {
                    ColumnCells.Add(newColumn.Cells);
                    ValidColumns.Columns.Add(newColumn);
                    newColumn.Print(Print);
                }
            }
        }
    }
}
