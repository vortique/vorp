using System;
using System.IO;
using System.Reflection.Metadata;
using System.Text;

namespace Vorp
{
    internal class Vorp
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                String filePath = args[0] == "-i" ? args[1] : args[0];
                bool ignoreCase = args[0] == "-i";
                String targetString = args[0] == "-i" ? args[2] : args[1];

                if (ignoreCase)
                {
                    targetString = targetString.ToLower();
                }

                try
                {
                    if (!Directory.Exists(filePath))
                    {
                        String line;

                        using (StreamReader sr = new StreamReader(filePath))
                        {
                            for (int i = 1; (line = sr.ReadLine()) != null; i++)
                            {
                                WriteFoundLines(line, targetString, i, ignoreCase);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"vorp ({filePath}): Given path is a directory");
                    }
                }
                catch (FileNotFoundException) // Exception handling when file not found
                {
                    Console.WriteLine($"vorp ({filePath}): No such file");
                }
                catch (IOException e)
                {
                    Console.WriteLine($"vorp: Something went wrong.\n\n{e}");
                }
            }
        }

        static void WriteFoundLines(String line, String targetString, int lineIndex, bool ignoreCase)
        {
            if (ignoreCase)
            {
                String lineLower = line.ToLower();
                bool found = lineLower.Contains(targetString);

                if (found)
                {
                    Console.WriteLine($" {lineIndex} | {line}");
                }
            }
            else
            {
                bool found = line.Contains(targetString);

                if (found)
                {
                    Console.WriteLine($" {lineIndex} | {line}");
                }
            }
        }
    }
}
