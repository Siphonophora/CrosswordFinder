using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordLibrary
{
    public class ColumnPickerLeft : IColumnPicker
    {
        /// <summary>
        /// Note, this picks only left columns in the first col, left and neighbors in 2 and 3, and neighbors in the last.
        /// </summary>
        /// <param name="columnGroup"></param>
        /// <param name="startPart"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public List<string> Pick(ColumnGroup columnGroup, string[] startPart, int col)
        {
            if (col == 0)
            {
                return columnGroup.Columns.Where(x => x.ValidLeftColumn).Select(y => y.ToString()).ToList();
            }
            
            if (col < 3)
            {
                var leftCols = columnGroup.Columns.Where(x => x.ValidLeftColumn).Select(y => y.ToString()).ToList();
                var neighbors = columnGroup.ColumnDictionary[startPart[col - 1]].Neighbors;

                return leftCols.Intersect(neighbors).ToList();
            }

            return columnGroup.ColumnDictionary[startPart[col - 1]].Neighbors;
        }
    }
}
