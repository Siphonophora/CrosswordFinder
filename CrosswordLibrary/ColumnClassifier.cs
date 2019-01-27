using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordLibrary
{
    public class ColumnClassifier
    {
        public ColumnClassifier(ColumnSet validColumns)
        {
            ValidColumns = validColumns ?? throw new ArgumentNullException(nameof(validColumns));

            Analyze();
        }
        public ColumnSet ValidColumns { get; set; }

        private void Analyze()
        {
            for (int i = 0; i < ValidColumns.Columns[0].Cells.Length; i++)
            {
                var parentOrder = ValidColumns.GetColumnsByOrder(i);
                var childOrder = ValidColumns.GetColumnsByOrder(i + 1);
                if(parentOrder.Count == 0 || childOrder.Count == 0)
                {
                    break;
                }

                foreach (var child in childOrder)
                {
                    foreach (var parent in parentOrder)
                    {
                        if(ValidLink(parent.ToString(), child.ToString()))
                        {
                            parent.Children.Add(child.ToString());
                            child.Parent = parent.ToString();
                            break;
                        }
                    }
                }
            }


            PrintClassifications();
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
                if(parent[i] == '1' && child[i] == '0')
                {
                    changes++;
                    if(changes > 1)
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
            //Console.WriteLine($"Symetric Columns: {ValidColumns.Columns.Where(x => x.IsSymetric).Count()}");
            Console.WriteLine($"ValidLeftColumn Columns: {ValidColumns.Columns.Where(x => x.ValidLeftColumn).Count()}");
            Console.WriteLine();
            Console.WriteLine($"NoBlackEdges Columns: {ValidColumns.Columns.Where(x => x.NoBlackEdges).Count()}");

            Console.WriteLine();
            Console.WriteLine("Possible base set");
            int moovlc = ValidColumns.Columns.Where(x => x.ValidLeftColumn).Max(y => y.Order);
            Console.WriteLine($"Max Order of ValidLeftColumns: {moovlc}");
            Console.WriteLine($"Total Columns: {ValidColumns.Columns.Where(x =>  x.Order <= moovlc).Count()}");
            //Console.WriteLine($"Symetric Columns: {ValidColumns.Columns.Where(x => x.IsSymetric && x.Order <= moovlc).Count()}");
            Console.WriteLine($"ValidLeftColumn Columns: {ValidColumns.Columns.Where(x => x.ValidLeftColumn && x.Order <= moovlc).Count()}");

        }
    }
}
