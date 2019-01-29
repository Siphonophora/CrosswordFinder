using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordLibrary
{
    public class PuzzleChecker
    {
        public Hashtable ColumnHashtable { get; set; } = new Hashtable();
        public long InvalidRowCount { get; set; } = 0;
        public long CheaterCount { get; set; } = 0;
        public long NotContiuousCount { get; set; } = 0;
        public int Size { get; set; } = 0;
        public int Mid { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderPuzzle"></param>
        /// <param name="candidate">Ref because we calculate word count in this </param>
        /// <returns></returns>
        public bool IsValidNewPuzzle(Puzzle candidate)
        {
            var cols = GetCols(candidate);

            for (int i = 0; i < candidate.Size; i++)
            {
                var row = GetRow(cols, i);
                var col = (Column)ColumnHashtable[row];
                if (col == null)
                {
                    //Catch when not a valid key
                    InvalidRowCount++;
                    return false;
                }

                //Test the special case that the three top and three bottom rows MUST be eligible to be in the left colum, or they are cheaters.
                if ((i <= 2 || i >= candidate.Size - 3) && col.ValidLeftColumn == false)
                {
                    InvalidRowCount++;
                    return false;
                }

            }

            if (HasCheater(cols))
            {
                CheaterCount++;
                return false;
            }

            if (NotContinous(cols))
            {
                NotContiuousCount++;
                //Console.WriteLine("NON Continuous Example");
                //Print(candidate);
                //Console.ReadKey();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines continuity of puzzle by searching first down then up to find all 
        /// </summary>
        /// <param name="cols"></param>
        /// <returns></returns>
        public bool NotContinous(string[] cols)
        {
            var words = new List<Word>();
            var colNum = cols.Length;
            var numContinuous = 0;
            var newContinuous = 0;

            for (int i = 0; i < cols.Length; i++)
            {
                List<Word> newWords = GetWords(cols[i], i);
                words.AddRange(newWords);
            }

            //First word is contiguous
            words.Where(x => x.Col == 0).First().Continuous = true;

            bool down = true;
            //Search in loops over the puzzle. We keepg going so long as we ARE finding new continuous 
            do
            {
                numContinuous += newContinuous;

                SearchContinuous(words, colNum, down);

                //If there are no non continuous in any row after the up search, then its non continuous
                if (words.Where(x => x.Continuous == false).Any() == false)
                {
                    return false;
                }

                down = !down;
                newContinuous = words.Where(x => x.Continuous).Count() - numContinuous;
            } while (newContinuous > 0);

            //If we didn't
            return true;
        }

        private void SearchContinuous(List<Word> words, int colNum, bool down)
        {
            if (down)
            {
                //Search Down. 
                for (int i = 0; i < colNum - 1; i++)
                {
                    var continuous = words.Where(x => x.Col == i && x.Continuous).ToArray();
                    var next = words.Where(x => x.Col == i + 1 && !x.Continuous).ToArray();
                    for (int j = 0; j < next.Count(); j++)
                    {
                        for (int k = 0; k < continuous.Count(); k++)
                        {
                            if (WordsOverlap(continuous[k], next[j]))
                            {
                                next[j].Continuous = true;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                //Search Up. 
                for (int i = colNum - 1; i >= 0; i--)
                {
                    var continuous = words.Where(x => x.Col == i && x.Continuous).ToArray();
                    var next = words.Where(x => x.Col == i - 1 && !x.Continuous).ToArray();
                    for (int j = 0; j < next.Count(); j++)
                    {
                        for (int k = 0; k < continuous.Count(); k++)
                        {
                            if (WordsOverlap(continuous[k], next[j]))
                            {
                                next[j].Continuous = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        public List<Word> GetWords(string col, int colNum = 0)
        {
            var words = new List<Word>();
            bool inWord = false;
            var currentWord = new Word();
            int i;
            for (i = 0; i < col.Length; i++)
            {

                if (inWord == false)
                {
                    if (col[i] == '1')
                    {
                        currentWord.Start = i;
                        currentWord.Col = colNum;
                        inWord = true;
                    }
                }
                else
                {
                    if (col[i] == '0')
                    {
                        currentWord.Stop = i - 1;
                        words.Add(currentWord);
                        currentWord = new Word(); //Do this here, because we have the function scoped variable
                        inWord = false;
                    }
                }
            }
            if (inWord)
            {
                currentWord.Stop = i - 1;
                words.Add(currentWord);
            }

            return words;
        }

        public bool WordsOverlap(Word a, Word b)
        {
            return a.Start <= b.Stop && b.Start <= a.Stop;
        }

        public class Word
        {
            public int Start;
            public int Stop;
            public int Col;
            public bool Continuous; //Default is false;

            public override string ToString()
            {
                return $"Col {Col}, Start {Start}, Stop {Stop}, Continuous {Continuous}";
            }
        }

        public string[] GetCols(Puzzle puzzle)
        {
            var cols = new string[puzzle.Size];

            for (int i = 0; i < puzzle.Columns.Length; i++)
            {
                if (i < puzzle.Columns.Length - 1)
                {
                    cols[i] = puzzle.Columns[i];
                    cols[puzzle.Size - 1 - i] = Reverse(puzzle.Columns[i]);
                }
                else
                {
                    cols[i] = ((Column)ColumnHashtable[puzzle.Columns[i]]).ToStringSymetric();
                }
            }

            return cols;
        }

        public string GetRow(string[] cols, int index)
        {
            var sb = new StringBuilder(Size);
            for (int i = 0; i < cols.Length; i++)
            {
                sb.Append(cols[i][index]);
            }

            return sb.ToString();
        }

        public string[] GetRows(string[] cols)
        {

            var rows = new string[cols.Length];
            var sb = new StringBuilder(Size);
            for (int i = 0; i < cols.Length; i++)
            {
                for (int j = 0; j < cols.Length; j++)
                {
                    sb.Append(cols[j][i]);
                }
                rows[i] = sb.ToString();
                sb.Clear();
            }

            return rows;
        }

        public bool HasCheater(string[] cols)
        {
            for (int i = 0; i < Mid; i++)
            {
                for (int j = 0; j < cols[0].Length; j++)
                {
                    if (cols[i][j] == '1')
                    {
                        continue; //this is a white cell, can't be a cheat
                    }

                    //Try all four types of 'L' cheaters. 
                    if (i > 0)
                    {
                        if (j > 0)
                        {
                            if (cols[i - 1][j] == '0' && cols[i][j - 1] == '0') { return true; }
                        }

                        if (j < Size - 1)
                        {
                            if (cols[i - 1][j] == '0' && cols[i][j + 1] == '0') { return true; }
                        }
                    }

                    if (i < Size - 1)
                    {
                        if (j > 0)
                        {
                            if (cols[i + 1][j] == '0' && cols[i][j - 1] == '0') { return true; }
                        }

                        if (j < Size - 1)
                        {
                            if (cols[i + 1][j] == '0' && cols[i][j + 1] == '0') { return true; }
                        }
                    }
                }
            }

            return false;
        }

        public string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public void Print(Puzzle candidate)
        {
            var cols = GetCols(candidate);
            int size = candidate.Size;
            Console.WriteLine(" ");
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write(" " + new string(' ', size) + " ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine($" Order: {candidate.Order}");
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Blue;
            for (int i = 0; i < size; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(" ");
                for (int j = 0; j < size; j++)
                {
                    if (cols[j][i] == '1')
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write("A");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(" ");
                    }
                }

                Console.BackgroundColor = ConsoleColor.Gray;
                Console.WriteLine(" ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write(" " + new string(' ', size) + " ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
            Console.WriteLine(" ");

        }
    }
}
