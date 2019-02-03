using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordLibrary
{
    public static class SVGWriter
    {
        public static void Write(string[] cols, string dir)
        {
            string binary = string.Join("", cols);
            binary = binary.Substring(0, (binary.Length + 1) / 2);
            var puzzleID = BinaryStringToHexString(binary);

            Directory.CreateDirectory(dir);


            WriteSVG(cols, $"{dir}\\{puzzleID}.svg");
        }

        private static void WriteSVG(string[] cols, string file)
        {
            int size = cols.Length;
            float mm = 5;
            float stroke = mm / 35;
            float font = mm / 5;
            int word = 1;


            var sb = new StringBuilder();
            sb.AppendLine($"<?xml version=\"1.0\" standalone=\"no\"?>");
            sb.AppendLine($"<!DOCTYPE svg PUBLIC \" -//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">");

            sb.AppendLine($"<svg width = \"{(mm * (size + 2)).ToString()}mm\" height = \"{(mm * (size + 2)).ToString()}mm\" version = \"1.1\" xmlns=\"http://www.w3.org/2000/svg\">");
            sb.AppendLine();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    float x = mm * (j + 1);
                    float y = mm * (i + 1);
                    bool black = cols[i][j] == '0';
                    sb.AppendLine($"<rect x=\"{x}mm\" y=\"{y}mm\" width=\"{mm}mm\" height=\"{mm}mm\" fill=\"{(black ? "black" : "none")}\" stroke=\"black\" stroke-width = \"{stroke.ToString("0.000")}mm\" />");

                    bool newWord = false;
                    if (!black)
                    {
                        if (i == 0)
                        {
                            newWord = true;
                        }
                        else if (cols[i - 1][j] == '0')
                        {
                            newWord = true;
                        }

                        if (j == 0)
                        {
                            newWord = true;
                        }
                        else if (cols[i][j - 1] == '0')
                        {
                            newWord = true;
                        }
                    }

                    if (newWord)
                    {
                        sb.AppendLine($"<text x=\"{x + stroke * 1.5}mm\" y=\"{y + font + stroke * 0.5}mm\" fill=\"black\" font-family=\"sans-serif\" font-size=\"{font}mm\">{word}</text>");
                        word++;
                    }
                }
                sb.AppendLine();
            }

            sb.AppendLine("</svg>");

            File.WriteAllText(file, sb.ToString());
        }

        public static string BinaryStringToHexString(string binary)
        {
            StringBuilder result = new StringBuilder(binary.Length / 8 + 1);

            // TODO: check all 1's or 0's... Will throw otherwise

            int mod4Len = binary.Length % 8;
            if (mod4Len != 0)
            {
                // pad to length multiple of 8
                binary = binary.PadLeft(((binary.Length / 8) + 1) * 8, '0');
            }

            for (int i = 0; i < binary.Length; i += 8)
            {
                string eightBits = binary.Substring(i, 8);
                result.AppendFormat("{0:X2}", Convert.ToByte(eightBits, 2));
            }

            return result.ToString();
        }
    }
}
