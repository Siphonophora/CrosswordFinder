using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CrosswordLibrary;

namespace CrosswordLibrary.UnitTests
{
    [TestFixture]
    public class ColumnTests
    {
        [TestCase(new bool[5] { false, false, false, false, false })]
        [TestCase(new bool[5] { true, false, false, false, false })]
        [TestCase(new bool[5] { false, true, false, false, false })]
        [TestCase(new bool[5] { false, false, true, false, false })]
        [TestCase(new bool[5] { false, false, false, true, false })]
        [TestCase(new bool[5] { false, false, false, false, true })]
        [TestCase(new bool[5] { true, true, false, true, true })]
        [TestCase(new bool[5] { false, false, true, false, false })]
        [TestCase(new bool[5] { false, false, true, false, false })]
        [TestCase(new bool[15] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false })]
        [TestCase(new bool[15] { false, true, false, true, false, true, false, true, false, false, false, false, false, false, false })]
        [TestCase(new bool[15] { true, true, false, false, false, false, false, false, false, false, false, false, false, false, false })]
        [TestCase(new bool[15] { false, false, false, false, false, false, false, false, false, false, false, false, false, true, true })]
        [TestCase(new bool[15] { false, true, true, false, true, true, false, false, false, false, false, false, false, false, false })]
        [TestCase(new bool[15] { true, true, false, true, true, false, false, false, false, false, true, true, false, true, true })]
        [TestCase(new bool[15] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false })]
        [TestCase(new bool[15] { true, true, true, true, true, true, false, true, true, false, true, true, true, true, true })]
        public void Column_Validator_IsInvalid(bool[] cells)
        {
            var c = new Column(cells);
            Assert.IsFalse(c.IsValid());
        }

        [TestCase(new bool[5] { true, true, true, true, true })]
        [TestCase(new bool[5] { false, true, true, true, true })]
        [TestCase(new bool[5] { false, false, true, true, true })]
        [TestCase(new bool[5] { true, true, true, true, false })]
        [TestCase(new bool[5] { true, true, true, false, false })]
        [TestCase(new bool[5] { false, true, true, true, false })]
        [TestCase(new bool[15] { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true })]
        [TestCase(new bool[15] { true, true, true, false, true, true, true, true, true, true, true, false, true, true, true })]
        [TestCase(new bool[15] { true, true, true, false, false, false, true, true, true, true, false, true, true, true, true })]
        [TestCase(new bool[15] { true, true, true, true, true, true, true, true, true, true, false, false, true, true, true })]
        [TestCase(new bool[15] { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true })]
        [TestCase(new bool[15] { true, true, true, false, false, true, true, true, false, true, true, true, false, false, false })]
        [TestCase(new bool[15] { false, false, false, false, false, false, false, false, true, true, true, false, false, false, false })]
        public void Column_Validator_IsValid(bool[] cells)
        {
            var c = new Column(cells);
            Assert.IsTrue(c.IsValid());
        }

        [TestCase(0, new bool[5] { true, true, true, true, true })]
        [TestCase(1, new bool[5] { false, true, true, true, true })]
        [TestCase(2, new bool[5] { false, false, true, true, true })]
        [TestCase(1, new bool[5] { true, true, true, true, false })]
        [TestCase(2, new bool[5] { true, true, true, false, false })]
        [TestCase(1, new bool[5] { false, true, true, true, false })]
        [TestCase(3, new bool[5] { false, true, false, false, false })]
        [TestCase(3, new bool[5] { false, false, false, true, false })]
        public void Column_MaxSequence_IsCorrect(int maxseq, bool[] cells)
        {
            var c = new Column(cells);
            Assert.AreEqual(maxseq, c.MaxBlackCellSequence());
        }

        [TestCase(true, new bool[5] { true, true, true, true, true })]
        [TestCase(true, new bool[5] { false, true, true, true, true })]
        [TestCase(true, new bool[5] { false, false, true, true, true })]
        [TestCase(true, new bool[5] { false, false, false, true, true })]
        [TestCase(false, new bool[5] { true, true, true, true, false })]
        [TestCase(false, new bool[5] { true, true, true, false, false })]
        [TestCase(false, new bool[5] { false, true, true, true, false })]
        [TestCase(false, new bool[5] { false, true, false, false, false })]
        [TestCase(false, new bool[5] { false, false, false, true, false })]
        public void Column_CenterEligible_IsCorrect(bool eligible, bool[] cells)
        {
            var c = new Column(cells);
            Assert.AreEqual(eligible, c.CanBeCenterColumn());
        }

        [TestCase("11111", new bool[5] { true, true, true, true, true })]
        [TestCase("01110", new bool[5] { false, true, true, true, true })]
        [TestCase("00100", new bool[5] { false, false, true, true, true })]
        [TestCase("00000", new bool[5] { false, false, false, true, true })]
        public void Column_ToStringSymetric_IsCorrect(string symetric, bool[] cells)
        {
            var c = new Column(cells);
            Assert.AreEqual(symetric, c.ToStringSymetric());
        }
    }
}
