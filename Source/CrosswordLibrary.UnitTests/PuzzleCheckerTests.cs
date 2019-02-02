using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrosswordLibrary;
using NUnit.Framework;

namespace CrosswordLibrary.UnitTests
{
    [TestFixture]
    class PuzzleCheckerTests
    {

        [TestCase("11111", "11111", "11111", "11111", "11111")]
        [TestCase("01111", "11111", "11111", "11111", "11111")]
        [TestCase("00111", "11111", "11111", "11111", "11111")]
        [TestCase("01111", "01111", "11111", "11111", "11111")]
        public void PuzzleChecker_Cheater_False(params string[] cols)
        {
            PuzzleChecker puzzleChecker = new PuzzleChecker(cols[0].Length);
            Assert.IsFalse(puzzleChecker.IsCheater(cols));
        }

        [TestCase("00111", "01111", "11111", "11111", "11111")]
        [TestCase("11100", "11110", "11111", "11111", "11111")]
        [TestCase("10111", "00111", "11111", "11111", "11111")]
        [TestCase("10111", "00111", "11111", "11111", "11111")]
        [TestCase("11111", "00111", "10111", "11111", "11111")]
        [TestCase("10111", "10011", "11111", "11111", "11111")]
        [TestCase("11111", "10011", "10111", "11111", "11111")]
        [TestCase("11111", "11011", "10011", "11111", "11111")]
        [TestCase("11111", "11111", "10011", "11011", "11111")]
        [TestCase("11111", "11011", "11001", "11111", "11111")]
        [TestCase("11111", "11111", "11001", "11011", "11111")]
        [TestCase("11111", "01111", "00111", "11111", "11111")]
        [TestCase("11111", "11110", "11100", "11111", "11111")]
        public void PuzzleChecker_Cheater_True(params string[] cols)
        {
            PuzzleChecker puzzleChecker = new PuzzleChecker(cols[0].Length);
            Assert.IsTrue(puzzleChecker.IsCheater(cols));
        }


        [TestCase(true, 0, 0, 0, 0)]
        [TestCase(true, 0, 1, 0, 0)]
        [TestCase(true, 0, 1, 1, 2)]
        [TestCase(true, 0, 2, 1, 1)]
        [TestCase(true, 1, 2, 0, 1)]
        [TestCase(false, 0, 0, 1, 1)]
        [TestCase(false, 0, 0, 1, 2)]
        [TestCase(false, 0, 0, 2, 2)]
        [TestCase(false, 2, 2, 0, 0)]
        [TestCase(false, 0, 1, 2, 2)]
        public void PuzzleChecker_NonContinous_True(bool assert, int a1, int a2, int b1, int b2)
        {
            int puzzleSize = 5;
            PuzzleChecker puzzleChecker = new PuzzleChecker(puzzleSize);
            Assert.AreEqual(assert, puzzleChecker.WordsOverlap(new PuzzleChecker.Word() { Start = a1, Stop = a2 }, new PuzzleChecker.Word() { Start = b1, Stop = b2 }));
        }

        [TestCase("11111", "11111", "00000", "11111", "11111")]
        [TestCase("11111", "11011", "10101", "11011", "11111")]
        [TestCase("01111", "10111", "11011", "11101", "11110")]
        [TestCase("01111", "10111", "01111", "11111", "11111")]
        [TestCase("01011", "10111", "11111", "11111", "11111")]
        [TestCase("11111", "10111", "11111", "11011", "10101")]
        [TestCase("10111", "01111", "11111", "11111", "11111")]
        public void PuzzleChecker_NonContinous_True(params string[] cols)
        {
            PuzzleChecker puzzleChecker = new PuzzleChecker(cols[0].Length);
            Assert.IsFalse(puzzleChecker.IsContinuous(cols));
        }

        [TestCase("11111", "11111", "11111", "11111", "11111")]
        [TestCase("11111", "01111", "11111", "11111", "11111")]
        [TestCase("11111", "11111", "11111", "11110", "11111")]
        [TestCase("11011", "11011", "11011", "11111", "11111")]
        [TestCase("11111", "11111", "11011", "11111", "11111")]
        [TestCase("11111", "11101", "11011", "10111", "11111")]
        [TestCase("01111", "10111", "11011", "11101", "11111")]
        public void PuzzleChecker_NonContinous_False(params string[] cols)
        {
            PuzzleChecker puzzleChecker = new PuzzleChecker(cols[0].Length);
            Assert.IsTrue(puzzleChecker.IsContinuous(cols));
        }


        [TestCase("111111111111111",
                      "111111111111111",
                      "111111111111111",
                      "111111111111111",
                      "000111101111000",
                      "111011101110111",
                      "111111011101111",
                      "111110111011111",
                      "111101110111111",
                      "111011101110111",
                      "000111101111000",
                      "111111111111111",
                      "111111111111111",
                      "111111111111111",
                      "111111111111111")]
        [TestCase("111111111111111",
                      "111111111111111",
                      "111111111111111",
                      "111111111111111",
                      "111111111111110",
                      "111100111110111",
                      "000111011111111",
                      "111111101111111",
                      "111111110111000",
                      "111011111001111",
                      "011111111111111",
                      "111111111111111",
                      "111111111111111",
                      "111111111111111",
                      "101111111111111")]

        [TestCase("101111011110111", // Trying to make one shaped like a maze      //" 0    0    0   "
                      "101001010010100",                                               //" 0 00 0 00 0 00"
                      "101001010010100",                                               //" 0 00 0 00 0 00"
                      "101001010010100",                                               //" 0 00 0 00 0 00"
                      "101001010010100",                                               //" 0 00 0 00 0 00"
                      "101101011010110",                                               //" 0  0 0  0 0  0"
                      "100101001010010",                                               //" 00 0 00 0 00 0"
                      "100101001010010",                                               //" 00 0 00 0 00 0"
                      "100101001010010",                                               //" 00 0 00 0 00 0"
                      "100101001010010",                                               //" 00 0 00 0 00 0"
                      "100101001010010",                                               //" 00 0 00 0 00 0"
                      "100101001010010",                                               //" 00 0 00 0 00 0"
                      "100101001010010",                                               //" 00 0 00 0 00 0"
                      "100101001010010",                                               //" 00 0 00 0 00 0"
                      "111101111011110")]                                              //"    0    0    0"
        public void PuzzleChecker_NonContinousRealWorld_False(params string[] cols)
        {
            PuzzleChecker puzzleChecker = new PuzzleChecker(cols[0].Length);
            Assert.IsTrue(puzzleChecker.IsContinuous(cols));
        }


        [TestCase(false, "11111", "11111")]
        [TestCase(false, "11111", "01111")]
        [TestCase(false, "11011", "11011")]
        [TestCase(false, "11111", "11101")]
        [TestCase(false, "01111", "10111")]
        [TestCase(true, "00111", "10111")]
        [TestCase(true, "01111", "00111")]
        [TestCase(true, "10011", "11011")]
        [TestCase(true, "11001", "11011")]
        [TestCase(true, "11011", "10011")]
        [TestCase(true, "11011", "11001")]
        [TestCase(true, "11100", "11110")]
        [TestCase(true, "11110", "11100")]
        public void PuzzleChecker_IsCheaterForNeighbors_Correct(bool assert, params string[] cols)
        {
            PuzzleChecker puzzleChecker = new PuzzleChecker(cols[0].Length);
            Assert.AreEqual(assert, puzzleChecker.IsCheater(cols));
        }

        //Shared partial rows
        [TestCase(true, "11111", "11111", "11111", "11111")]
        [TestCase(true, "01111", "01111", "01111", "01111")]
        [TestCase(false, "11011", "11111", "11111", "11011")]
        //Different 
        [TestCase(true, "01111", "10111", "11011", "11100")]
        [TestCase(true, "10111", "10111", "10101", "11101")]
        [TestCase(true, "11111", "10111", "11111", "11111")]
        [TestCase(true, "11111", "10111", "10111", "11111")]
        [TestCase(true, "11111", "11111", "11011", "11111")]
        public void PuzzleChecker_PartialRowsAreValid_Correct(bool assert, params string[] cols)
        {
            PuzzleChecker puzzleChecker = new PuzzleChecker(cols[0].Length);
            Assert.AreEqual(assert, puzzleChecker.PartialRowsAreValid(cols, false));
        }

        //Shared partial rows
        [TestCase(true, "11111", "11111", "11111", "11111")]
        [TestCase(true, "01111", "01111", "01111", "01111")]
        [TestCase(false, "11011", "11111", "11111", "11011")]
        //Different 
        [TestCase(false, "01111", "10111", "11011", "11100")]
        [TestCase(false, "10111", "10111", "10101", "11101")]
        [TestCase(false, "11111", "10111", "11111", "11111")]
        [TestCase(false, "11111", "10111", "10111", "11111")]
        [TestCase(false, "11111", "11111", "11011", "11111")]
        public void PuzzleChecker_PartialRowsAreValid_LeftEdge_Correct(bool assert, params string[] cols)
        {
            PuzzleChecker puzzleChecker = new PuzzleChecker(cols[0].Length);
            Assert.AreEqual(assert, puzzleChecker.PartialRowsAreValid(cols, true));
        }
    }
}
