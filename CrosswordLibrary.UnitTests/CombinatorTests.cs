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
    class CombinatorTests
    {
        [Test]
        public void CombinatorTests_1_Pass()
        {
            var start = new string[] { "a" };
            var options = new string[][] {
                new string[] { "a" }
            };

            var c = Combinator.FindCombinations(start, options, 0, false);
            Assert.AreEqual(1, c.Count);
        }

        [Test]
        public void CombinatorTests_3_Pass()
        {
            var start = new string[] { "a" };
            var options = new string[][] {
                new string[] { "a", "b", "c" }
            };

            var c = Combinator.FindCombinations(start, options, 0, false);
            Assert.AreEqual(3, c.Count);
        }

        [Test]
        public void CombinatorTests_3x3_Pass()
        {
            var start = new string[] { "a", "b" };
            var options = new string[][] {
                new string[] { "a", "aa", "aaa" },
                new string[] { "b", "bb", "bbb" }
            };

            var c = Combinator.FindCombinations(start, options, 0, false);
            Assert.AreEqual(9, c.Count);
        }


        [Test]
        public void CombinatorTests_2x1x2_Pass()
        {
            var start = new string[] { "a", "b", "c" };
            var options = new string[][] {
                new string[] { "a", "aa" },
                new string[] { "b" },
                new string[] { "c", "cc" }
            };

            var c = Combinator.FindCombinations(start, options, 0, false);
            Assert.AreEqual(4, c.Count);
        }

        [Test]
        public void CombinatorTests_3x3x1x4x2_Pass()
        {
            var start = new string[] { "a", "b", "c", "d", "e" };
            var options = new string[][] {
                new string[] { "a", "aa", "aaa" },
                new string[] { "b", "bb", "bbb" },
                new string[] { "c" },
                new string[] { "d", "dd", "ddd", "dddd" },
                new string[] { "e", "ee" }
            };

            var c = Combinator.FindCombinations(start, options, 0, false);
            Assert.AreEqual(72, c.Count);
        }


        [Test]
        public void CombinatorTests_SuppressStart_1_Pass()
        {
            var start = new string[] { "a" };
            var options = new string[][] {
                new string[] { "a" }
            };

            var c = Combinator.FindCombinations(start, options);
            Assert.AreEqual(0, c.Count);
        }

        [Test]
        public void CombinatorTests_SuppressStart_3_Pass()
        {
            var start = new string[] { "a" };
            var options = new string[][] {
                new string[] { "a", "b", "c" }
            };

            var c = Combinator.FindCombinations(start, options);
            Assert.AreEqual(2, c.Count);
        }

        [Test]
        public void CombinatorTests_SuppressStart_3x3_Pass()
        {
            var start = new string[] { "a", "b" };
            var options = new string[][] {
                new string[] { "a", "aa", "aaa" },
                new string[] { "b", "bb", "bbb" }
            };

            var c = Combinator.FindCombinations(start, options);
            Assert.AreEqual(8, c.Count);
        }


        [Test]
        public void CombinatorTests_SuppressStart_2x1x2_Pass()
        {
            var start = new string[] { "a", "b", "c" };
            var options = new string[][] {
                new string[] { "a", "aa" },
                new string[] { "b" },
                new string[] { "c", "cc" }
            };

            var c = Combinator.FindCombinations(start, options);
            Assert.AreEqual(3, c.Count);
        }

        [Test]
        public void CombinatorTests_SuppressStart_3x3x1x4x2_Pass()
        {
            var start = new string[] { "a", "b", "c", "d", "e" };
            var options = new string[][] {
                new string[] { "a", "aa", "aaa" },
                new string[] { "b", "bb", "bbb" },
                new string[] { "c" },
                new string[] { "d", "dd", "ddd", "dddd" },
                new string[] { "e", "ee" }
            };

            var c = Combinator.FindCombinations(start, options);
            Assert.AreEqual(71, c.Count);
        }

        [Test]
        public void CombinatorTests_SuppressStart_Int2x2_Pass()
        {
            var start = new int[] { 1, 2 };
            var options = new int[][] {
                new int[] { 1,11 },
                new int[] { 2,22}
            };

            var c = Combinator.FindCombinations(start, options);
            Assert.AreEqual(3, c.Count);
        }

        [Test]
        public void CombinatorTests_SuppressStart_Bool2x2_Pass()
        {
            var start = new bool[] { true, true };
            var options = new bool[][] {
                new bool[] {true, false },
                new bool[] {true, false }
            };

            var c = Combinator.FindCombinations(start, options);
            Assert.AreEqual(3, c.Count);
        }
    }
}
