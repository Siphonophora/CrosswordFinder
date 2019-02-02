using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordLibrary
{
    public class ColumnGroup
    {
        public ColumnGroup(int columnSize)
        {
            ColumnSize = columnSize;
        }

        public void Add(Column newCol)
        {
            Columns.Add(newCol);
            ColumnDictionary.Add(newCol.ToString(), newCol);
        }

        public List<Column> Columns { get; private set; } = new List<Column>();
        public Dictionary<string, Column> ColumnDictionary { get; private set; } = new Dictionary<string, Column>();

        public int ColumnSize { get; private set; }

        public void PrintInfo()
        {
            Console.WriteLine();
            Console.WriteLine($"In total there are {Columns.Count} columns with {Columns.Sum(x => x.Neighbors.Count)} Neighbors");
            Console.WriteLine();
        }
    }
}
