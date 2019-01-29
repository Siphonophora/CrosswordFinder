using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordLibrary
{
    public struct Puzzle
    {
        public Puzzle(int size) : this()
        {
            Size = size;
            Columns = new string[(size + 1) / 2];
        }

        public int Size { get; private set; }

        /// <summary>
        /// Note the last element in Columns isn't the actual column, it is replaced with the symetric string
        /// </summary>
        public string[] Columns { get; set; }
        public int Order { get; set; }
        public int WordCount { get; set; }
    }
}
