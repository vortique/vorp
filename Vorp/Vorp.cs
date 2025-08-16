using System;
using System.IO;
using System.Text;

namespace Vorp
{
    internal class Vorp
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                String filePath = args[0];

                try
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    long fileSize = fileInfo.Length;

                    int bufferSize = GetBufferSize(fileSize);

                    using (FileStream fs = new FileStream(filePath,
                        FileMode.Open, FileAccess.Read))
                    {
                        UTF8Encoding utf8 = new UTF8Encoding();
                        byte[] buffer = new byte[bufferSize];
                        int bytesReaded;
                        while ((bytesReaded = fs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            String decodedText = utf8.GetString(buffer, 0, bytesReaded);
                            Console.Write(decodedText);
                        }
                        Console.WriteLine();
                    }
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine($"vorp ({filePath}): No such file or directory");
                }
                catch (IOException e)
                {
                    Console.WriteLine($"vorp: Something went wrong.\n\n{e}");
                }
            }
        }

        static int GetBufferSize(long fileSize)
        {
            if (fileSize < 1024)
            {
                return 1024;
            }
            else if (fileSize < 1024 * 1024)
            {
                return 6 * 1024;
            }
            else
            {
                return 64 * 1024;
            }
        }
    }
}
