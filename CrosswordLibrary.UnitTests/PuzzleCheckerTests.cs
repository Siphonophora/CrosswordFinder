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
            int puzzleSize = cols[0].Length;
            PuzzleChecker puzzleChecker = new PuzzleChecker
            {
                Size = puzzleSize,
                Mid = (puzzleSize + 1) / 2
            };
            Assert.IsFalse(puzzleChecker.HasCheater(cols));
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
            int puzzleSize = cols[0].Length;
            PuzzleChecker puzzleChecker = new PuzzleChecker
            {
                Size = puzzleSize,
                Mid = (puzzleSize + 1) / 2
            };
            Assert.IsTrue(puzzleChecker.HasCheater(cols));
        }
    }
}
