using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrosswordLibrary;

namespace CrosswordPuzzle
{
    class Program
    {
        static void Main(string[] args)
        {

            //var bits = new BitArray(4);
            //bool[] myBools = new bool[15] { false, true, true, true, true, false, false, false, false, true, true, true, true, true, true };

            //var c = new Column(myBools);
            //c.Print();

            //var cf5 = new ColumnFinder(5);
            //var cc5 = new ColumnClassifier(cf5.ValidColumns);
            //var pf5_7 = new PuzzleFinder(cf5.ValidColumns, 7, true);

            //Total found with cheaters ~350
            //Total without cheaters 6
            var cf7 = new ColumnFinder(7);
            var cc7 = new ColumnClassifier(cf7.ValidColumns);
            var pf7_7 = new PuzzleFinder(cf7.ValidColumns,5, true);

            //var cf11 = new ColumnFinder(11);
            //var cc11 = new ColumnClassifier(cf11.ValidColumns);
            //var pf11_7 = new PuzzleFinder(cf11.ValidColumns, 15, true);


            //Total possible if using cheaters 1.5e16
            //Actual Total found by other participant 4e10
            //Total possible without chetaers 1.5e12 ==  14^ 3 * 304 ^ 3 * 20 

            //var cf13 = new ColumnFinder(13);
            //var cc13 = new ColumnClassifier(cf13.ValidColumns);
            //var pf13_7 = new PuzzleFinder(cf13.ValidColumns, 15, true);






            //Checked about 20k puzzels per second. 

            //Total possible 26^3 * 797^4 * 33 
            //About 2e17
            //Total in 'enumerable set' 26^3 * 141^4 * 9 
            //About 6e13
            //var cf15 = new ColumnFinder(15);
            //var cc15 = new ColumnClassifier(cf15.ValidColumns);
            //var pf15 = new PuzzleFinder(cf15.ValidColumns, 4, true);

            //Total possible 181^3 * 14327^7 * 143 
            //About 1e38
            //var cf21 = new ColumnFinder(21);
            //var cc21 = new ColumnClassifier(cf21.ValidColumns);


            Console.WriteLine("Thats it");
            Console.ReadLine();
        }
    }
}
