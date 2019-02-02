using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordLibrary
{
    public interface IColumnPicker
    {
        List<string> Pick(ColumnGroup columnGroup, string[] startPart, int col);
    }
}
