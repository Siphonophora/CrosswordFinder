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
    class CombinatorSlicesTests
    {
        
        [Test]
        public void CombinatorTests_Slices_3x3_Pass()
        {
            var start = new string[] { "a", "b" };
            var options = new string[][] {
                new string[] { "a", "aa", "aaa" },
                new string[] { "b", "bb", "bbb" }
            };

            var c = Combinator.FindCombinations(start, options, 0, true, new int[] { 0 });
            Assert.AreEqual(2, c.Count);
        }

        [Test]
        public void CombinatorTests_Slices_3x3_DoesntUseStart_Pass()
        {
            var start = new string[] { "a", "b" };
            var options = new string[][] {
                new string[] { "a", "aa", "aaa" },
                new string[] { "b", "bb", "bbb" }
            };

            var c = Combinator.FindCombinations(start, options, 0, true, new int[] { 1 });
            Assert.AreEqual(3, c.Count);
        }


        [Test]
        public void CombinatorTests_Slices_2x1x2_Pass()
        {
            var start = new string[] { "a", "b", "c" };
            var options = new string[][] {
                new string[] { "a", "aa" },
                new string[] { "b" },
                new string[] { "c", "cc" }
            };

            var c = Combinator.FindCombinations(start, options, 0, true, new int[] { 0 });
            Assert.AreEqual(1, c.Count);
        }

        [Test]
        public void CombinatorTests_Slices_3x3x1x4x2_Pass()
        {
            var start = new string[] { "a", "b", "c", "d", "e" };
            var options = new string[][] {
                new string[] { "a", "aa", "aaa" },
                new string[] { "b", "bb", "bbb" },
                new string[] { "c" },
                new string[] { "d", "dd", "ddd", "dddd" },
                new string[] { "e", "ee" }
            };

            var c = Combinator.FindCombinations(start, options, 0, true, new int[] { 0 });
            Assert.AreEqual(23, c.Count);
        }

        [Test]
        public void CombinatorTests_Slices_3x3x1x4x2_TwoSlices()
        {
            var start = new string[] { "a", "b", "c", "d", "e" };
            var options = new string[][] {
                new string[] { "a", "aa", "aaa" },
                new string[] { "b", "bb", "bbb" },
                new string[] { "c" },
                new string[] { "d", "dd", "ddd", "dddd" },
                new string[] { "e", "ee" }
            };

            var c = Combinator.FindCombinations(start, options, 0, true, new int[] { 0, 0 });
            Assert.AreEqual(7, c.Count);
        }

        [Test]
        public void CombinatorTests_Slices_3x3x1x4x2_DoesntUseStart_TwoSlices()
        {
            var start = new string[] { "a", "b", "c", "d", "e" };
            var options = new string[][] {
                new string[] { "a", "aa", "aaa" },
                new string[] { "b", "bb", "bbb" },
                new string[] { "c" },
                new string[] { "d", "dd", "ddd", "dddd" },
                new string[] { "e", "ee" }
            };

            var c = Combinator.FindCombinations(start, options, 0, true, new int[] { 1,1 });
            Assert.AreEqual(8, c.Count);
        }


    }
}
