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

                try
                {
                    if (!Directory.Exists(filePath))
                    {
                        FileInfo fileInfo = new FileInfo(filePath); // Getting file's info. Especially file's Length in bytes for dynamic buffer size.
                        long fileSize = fileInfo.Length;

                        int bufferSize = GetBufferSize(fileSize);

                        using (FileStream fs = new FileStream(filePath,
                            FileMode.Open, FileAccess.Read))
                        {
                            UTF8Encoding utf8 = new UTF8Encoding();
                            byte[] buffer = new byte[bufferSize];
                            int bytesReaded;
                            String currentLine = "";
                            while ((bytesReaded = fs.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                int indexOfNewline = Array.IndexOf(buffer, 0x0A, 0, bytesReaded);

                                if (indexOfNewline != -1)
                                {
                                    currentLine += utf8.GetString(buffer, 0, indexOfNewline);
                                    WriteFoundLines(currentLine, targetString, ignoreCase);
                                    currentLine = "";
                                }
                                else
                                {
                                    currentLine += utf8.GetString(buffer, 0, bytesReaded);
                                }
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

        // Dynamic buffer size for optimizing RAM usage.
        static int GetBufferSize(long fileSize)
        {
            if (fileSize < 1024) // 1 kB
            {
                return 1024;
            }
            else if (fileSize < 1024 * 1024) // 1 mB
            {
                return 6 * 1024;
            }
            else // Larger than 1 mB
            {
                return 64 * 1024;
            }
        }

        static void WriteFoundLines(String line, String targetString, bool ignoreCase)
        {
            if (ignoreCase)
            {
                String lineLower = line.ToLower();
                String targetLower = targetString.ToLower();
                bool found = lineLower.Contains(targetLower);

                if (found)
                {
                    Console.WriteLine(line);
                }
            }
            else
            {
                bool found = line.Contains(targetString);

                if (found)
                {
                    Console.WriteLine(line);
                }
            }
        }
    }
}
