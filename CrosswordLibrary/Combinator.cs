using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordLibrary
{
    public static class Combinator
    {
        public static List<T[]> FindCombinations<T>(T[] start, T[][] options, int depth = 0, bool suppressStart = true)
        {
            var results = new List<T[]>();
            var size = options.Length;

            foreach (var option in options[depth])
            {
                var newOption = start.ToArray();
                newOption[depth] = option;
                if (depth == size - 1)
                {
                    results.Add(newOption);
                }
                else
                {
                    results.AddRange(FindCombinations<T>(newOption, options, depth + 1, false));
                }
            }

            if (suppressStart)
            {
                results = results.Where(x => !x.SequenceEqual(start)).ToList() ;
            }
            return results;
        }
    }
}
