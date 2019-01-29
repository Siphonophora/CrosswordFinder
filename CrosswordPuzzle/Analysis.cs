using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrosswordLibrary;

namespace CrosswordPuzzle
{
    public static class Analysis
    {
        public static void Run(int size, int slice)
        {
            Console.WindowWidth = 200;
            Console.WindowHeight = 70;

            var cf15 = new ColumnFinder(size);
            var cc15 = new ColumnClassifier(cf15.ValidColumns);
            var pf15 = new PuzzleFinder(cf15.ValidColumns, size + 1, true, (size - 1) / 2, slice);
        }
    }
}
