using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace CrosswordLibrary
{
    public static class ResultSumarizer
    {
        public static void Summarize(string dir)
        {
            var files = Directory.GetFiles(dir, "*.json");
            var outdir = $"{dir}\\summary";
            Directory.CreateDirectory(outdir);
            var outfile = $"{outdir}\\summary.json";

            var results = new List<PuzzleFinder.PuzzleCheckStats>();

            foreach (var file in files)
            {
                var json = File.ReadAllText(file);
                results.Add(JsonConvert.DeserializeObject<PuzzleFinder.PuzzleCheckStats>(json));
            }

            var summary = new PuzzleFinder.PuzzleCheckStats() { Label = $"Summary of {files.Count()} join column's checked" };
            summary.Seconds = results.Sum(x => x.Seconds);
            summary.Valid = results.Sum(x => x.Valid);
            summary.Checked = results.Sum(x => x.Checked);

            results.Add(summary);

            var jsonout = JsonConvert.SerializeObject(results, Formatting.Indented);
            File.WriteAllText(outfile, jsonout);

            Console.ReadLine();
        }
    }
}
