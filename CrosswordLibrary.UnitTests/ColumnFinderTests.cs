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
    public class ColumnFinderTests
    {
        [TestCase(5)]
        [TestCase(7)]
        public void Column_Validator_IsInvalid(int colSize)
        {
            var cf = new ColumnFinder(colSize);
            
            Assert.AreEqual(cf.ColumnCells.Count, 0);
        }
    }
}
