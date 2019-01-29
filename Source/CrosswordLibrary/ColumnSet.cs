using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordLibrary
{
    public class ColumnSet
    {
        public ColumnSet(int columnSize)
        {
            ColumnSize = columnSize;
        }

        public List<Column> Columns { get; set; } = new List<Column>();
        public int ColumnSize { get; private set; }

        public List<Column> GetColumnsByOrder(int order)
        {
            return Columns.Where(x => x.Order == order).ToList();
        }

        public void PrintInfo()
        {
            Console.WriteLine();
            Console.WriteLine($"In total there are {Columns.Count} columns with {Columns.Sum(x => x.Children.Count)} children");
            Console.WriteLine();
            var cols = new List<Column>();
            int i = 0;
            do
            {
                cols = GetColumnsByOrder(i);
                var children = cols.Sum(x => x.Children.Count);

                Console.WriteLine($"Col Order {i.ToString().PadLeft(2)} have {children.ToString().PadLeft(2)} children");
                i++;
            } while (cols.Count > 0);
        }
    }
}
