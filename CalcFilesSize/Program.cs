//Варіант 16. Підрахувати сумарний обсяг файлів в каталозі.
using System.IO;
using System;

internal class Program
{
    private const int ERROR = 0xA0;
    static string[] fullfilesPath;
    static string mask = "*.*";
    static string path = Directory.GetCurrentDirectory();
    static FileAttributes attr = 0;
    static bool helpMode = false;

    static void GetVolume(string[] fullfilesPath, FileAttributes attr)
    {
        long fullSize = 0;
        foreach (string str in fullfilesPath)
        {
            FileInfo fileInfo = new FileInfo(str);
            if (attr == 0 || File.GetAttributes(str).HasFlag(attr))
            { fullSize += fileInfo.Length; }
        }
        Console.WriteLine($"the total volume of files in the directory using {mask} mask: ");
        Console.WriteLine(fullSize);
    }

    static void ParseArgs(string[] args)
    {
        if (args.Length == 1 && args[0] == "/?")
        {
            GetHelp();
            helpMode = true;
        } 
        else if (args.Length % 2 == 0)
        {
            for (int i = 0; i < args.Length; i += 2)
            {
                switch(args[i])
                {
                    case "/p":
                        path = args[i + 1]; break;
                    case "/m":
                        mask = args[i + 1]; break;
                    case "/a":
                        switch (args[i + 1])
                        {
                            case "ReadOnly":
                                attr = FileAttributes.ReadOnly; break;
                            case "Hidden":
                                attr = FileAttributes.Hidden; break;
                            case "System":
                                attr = FileAttributes.System; break;
                            case "Archive":
                                attr = FileAttributes.Archive; break;
                        } break;
                }
            }
        }
        else
        {
            Console.WriteLine("arguments are not correct");
            Environment.ExitCode = ERROR;
        }
    }

    static void GetHelp()
    {
        Console.WriteLine("counting the total volume of files in the directory");
        Console.WriteLine("usage: CaclFilesSize [/p] [path_to_directory] [/m] [file_mask] [/a] [file_attribute]");
        Console.WriteLine("path_to_directory: if empty, using current directory");
        Console.WriteLine("file_mask: if empty, using *.*");
        Console.WriteLine("file_attribute: if empty, using all attributes. attributes that you can use: ReadOnly, Hidden, System, Archive");
    }

    private static void Main(string[] args)
     {
        ParseArgs(args);
        if (!helpMode)
        {
            if (Directory.Exists(path))
            {
                fullfilesPath = Directory.GetFiles(path, mask, SearchOption.AllDirectories);
                if (fullfilesPath.Length == 0) { Console.WriteLine("no files found"); }
                else
                {
                    GetVolume(fullfilesPath, attr);
                }
            }
            else { Environment.ExitCode = ERROR; }
        }
    }
}