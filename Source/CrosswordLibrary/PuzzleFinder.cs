﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Serilog;

namespace CrosswordLibrary
{
    public class PuzzleFinder
    {
        public ColumnGroup ValidColumns { get; private set; }
        public int Size { get; }
        public string LeftDir { get; }
        public string CenterDir { get; }
        public string ValidDir { get; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public string[] LastValidPuzzle { get; set; }


        public PuzzleFinder(PuzzleChecker pc, ColumnGroup validColumns, int size, string leftDir, string centerDir, string validDir)
        {
            ValidColumns = validColumns;
            Size = size;
            LeftDir = leftDir;
            CenterDir = centerDir;
            ValidDir = validDir;

            var leftFiles = Directory.GetFiles(leftDir, "*.json");
            var cntrFiles = Directory.GetFiles(centerDir, "*.json");

            int i = 0;
            foreach (var leftFile in leftFiles)
            {
                var validFiles = Directory.GetFiles(validDir, "*.inprocess");
                if (cntrFiles.Where(x => Path.GetFileName(x) == Path.GetFileName(leftFile)).Count() > 0
                   && validFiles.Where(x => Path.GetFileName(x) == Path.GetFileName(leftFile)).Count() == 0
                  )
                {
                    File.Create($"{validDir}\\{Path.GetFileNameWithoutExtension(leftFile)}.inprocess");
                    var stats = FindPuzzels(pc, validColumns, leftFile, cntrFiles.Where(x => Path.GetFileName(x) == Path.GetFileName(leftFile)).First(), validDir);
                    Console.WriteLine($"Done With {Path.GetFileName(leftFile)}. File {i} of {leftFiles.Count()}");
                    File.WriteAllText($"{validDir}\\{Path.GetFileNameWithoutExtension(leftFile)}.json", JsonConvert.SerializeObject(stats, Formatting.Indented));
                    LogStatus(pc);
                }
                else
                {
                    Console.WriteLine($"Skipping already run {Path.GetFileName(leftFile)}. File {i} of {leftFiles.Count()}");
                }

                i++;
            }
        }

        private PuzzleCheckStats FindPuzzels(PuzzleChecker pc, ColumnGroup validColumns, string leftFile, string centerFile, string validDir)
        {
            var leftPuzzleParts = PartsFromJson(leftFile);
            var centerPuzzleParts = PartsFromJson(centerFile);
            var stats = new PuzzleCheckStats() { Valid = 0, Checked = 0, Label = Path.GetFileNameWithoutExtension(leftFile) };


            var validLeftColumnsDict = new Dictionary<string, Column>();
            var validColumnsDict = new Dictionary<string, Column>();
            for (int i = 0; i < ValidColumns.Columns.Count; i++)
            {
                if (ValidColumns.Columns[i].ValidLeftColumn)
                {
                    validLeftColumnsDict.Add(ValidColumns.Columns[i].ToString(), ValidColumns.Columns[i]);
                }
                validColumnsDict.Add(ValidColumns.Columns[i].ToString(), ValidColumns.Columns[i]);
            }

            for (int i = 0; i < leftPuzzleParts.Count; i++)
            {
                for (int j = 0; j < centerPuzzleParts.Count; j++)
                {
                    stats.Checked++;
                    var cols = ColsFromParts(leftPuzzleParts[i], centerPuzzleParts[j]);
                    if (pc.IsValidPuzzle(validColumnsDict, validLeftColumnsDict, cols))
                    {
                        LastValidPuzzle = cols;
                        stats.Valid++;
                    }

                    if (pc.PuzzlesChecked % 10_000_000 == 0)
                    {
                        WriteStatus(pc);
                        pc.Print(LastValidPuzzle);
                    }
                }
            }
            return stats;
        }

        public struct PuzzleCheckStats
        {
            public string Label;
            public int Valid;
            public int Checked;
        }

        private string[] ColsFromParts(string[] left, string[] center)
        {
            var size = left[0].Length;
            var cols = new string[size];

            for (int i = 0; i < left.Length; i++)
            {
                cols[i] = left[i];
                cols[size - i - 1] = Reverse(left[i]);
            }

            var offset = left.Length;
            for (int i = 1; i < center.Length; i++) //Start at 1 because of column overlap
            {
                cols[i + offset - 1] = center[i];
                cols[size - (i + offset - 1) - 1] = Reverse(center[i]);
            }

            return cols;
        }



        public string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }



        private List<string[]> PartsFromJson(string file)
        {
            var json = File.ReadAllText(file);
            return JsonConvert.DeserializeObject<List<string[]>>(json);
        }

        private void WriteStatus(PuzzleChecker pc)
        {
            int pad = 30;

            Console.WriteLine("------------------------------Status--------------------------------------------");
            Console.WriteLine($" Elapsed Seconds       {(DateTime.Now - StartTime).TotalSeconds.ToString("0.0").PadLeft(pad, ' ')}");
            Console.WriteLine($" Puzzels Checked       {pc.PuzzlesChecked.ToString("N0").PadLeft(pad, ' ')}");
            Console.WriteLine($" Total Edge Row:       {(pc.InvalidEdgeRowCount).ToString("N0").PadLeft(pad, ' ')} ");
            Console.WriteLine($" Total Invalid Row:    {(pc.InvalidRowCount).ToString("N0").PadLeft(pad, ' ')} ");
            Console.WriteLine($" Total Cheater:        {(pc.CheaterCount).ToString("N0").PadLeft(pad, ' ')} ");
            Console.WriteLine($" Total Not Continuous: {(pc.NotContiuousCount).ToString("N0").PadLeft(pad, ' ')} ");
            Console.WriteLine($" Total Valid:          {(pc.ValidCount).ToString("N0").PadLeft(pad, ' ')}");
            Console.WriteLine("---------------------------------------------------------------------------------");

            LogStatus(pc);
        }

        private void LogStatus(PuzzleChecker pc)
        {
            Log.Logger.Information(" Elapsed Seconds {sec}" +
           " Puzzels Checked {puzzelschecked}" +
           " Total Invalid Edge Row: {totalInvalidEdgeRows}" +
           " Total Invalid Row: {totalinvalidrows}" +
           " Total Cheater: {totalcheater}" +
           " Total Not Continuous: {totalnotcontinuous}" +
           " Total Valid: {totalvalid}"

            , (DateTime.Now - StartTime).TotalSeconds
            , pc.PuzzlesChecked
            , pc.InvalidEdgeRowCount
            , pc.InvalidRowCount
            , pc.CheaterCount
            , pc.NotContiuousCount
            , pc.ValidCount
            );

        }
    }
}
