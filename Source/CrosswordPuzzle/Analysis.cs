using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrosswordLibrary;
using Newtonsoft.Json;

namespace CrosswordPuzzle
{
    public static class Analysis
    {
        public static void Run(int size, int slice, string folder)
        {
            Console.WindowWidth = 200;
            Console.WindowHeight = 70;

            var cf = new ColumnFinder(size);
            var cc = new ColumnClassifier(cf.ValidColumns);
            var ppf = new PuzzlePartFinder();
            var pc = new PuzzleChecker(size);

            var leftDir = $"{folder}\\LeftPuzzleParts";
            var centerDir = $"{folder}\\CenterPuzzleParts";
            var validDir = $"{folder}\\ValidPuzzles"; Directory.CreateDirectory(centerDir);
            Directory.CreateDirectory(leftDir);
            Directory.CreateDirectory(centerDir);
            Directory.CreateDirectory(validDir);


            //var leftParts = ppf.FindParts(cc.ValidColumns, Enums.PuzzlePart.Left, new ColumnPickerLeft(), size);
            //WritePartsToDisk(leftParts, Enums.Column.Last, $"{folder}\\LeftPuzzleParts");

            //var centerParts = ppf.FindParts(cc.ValidColumns, Enums.PuzzlePart.Center, new ColumnPickerCenter(), size);
            //WritePartsToDisk(centerParts, Enums.Column.First, $"{folder}\\CenterPuzzleParts");

            var pf = new PuzzleFinder(pc, cc.ValidColumns, size, leftDir, centerDir, validDir);
        }

        private static void WritePartsToDisk(List<string[]> centerParts, Enums.Column keyCol, string folder)
        {
            int keyColNum = keyCol == Enums.Column.First ? 0 : centerParts[0].Length - 1;
            var keyCols = centerParts.Select(x => x[keyColNum]).Distinct().ToList();

            string json = "";
            foreach (var key in keyCols)
            {
                var cols = centerParts.Where(x => x[keyColNum] == key);
                json = JsonConvert.SerializeObject(cols, Formatting.Indented);
                File.WriteAllText($"{folder}\\{key}.json", json);
                Console.WriteLine($"{key} has {cols.Count()} members");
            }
        }
    }
}
