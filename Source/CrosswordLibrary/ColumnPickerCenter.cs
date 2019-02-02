using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordLibrary
{
    public class ColumnPickerCenter : IColumnPicker
    {
        //N.B. We pick in reverse order, from the center going to the left. 
        public List<string> Pick(ColumnGroup columnGroup, string[] startPart, int col)
        {
            if(col == 0)
            {
                return columnGroup.Columns.Where(x => x.CanBeCenterColumn()).Select(y => y.ToString()).ToList();
            }

            return columnGroup.ColumnDictionary[startPart[col - 1]].Neighbors;
        }
    }
}
