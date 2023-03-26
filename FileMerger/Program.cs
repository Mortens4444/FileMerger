﻿namespace MergeTextFiles;

class Program
{
    private static readonly string[] SearchPatterns = new string[] { "*.asm", "*.pas", "*.c", "*.cpp", "*.c++", "*.java", "*.vb", "*.cs", "*.ts", "*.json" };
    private static readonly string[] Excludeditems = new string[] { "obj", "bin", "node_modules", "package-lock.json", ".eslintrc.json" };

    static void Main(string[] args)
    {
        var inputFoldersAndFiles = new List<string>();

        if (args.Length >= 0)
        {
            inputFoldersAndFiles = args.Where(arg => Directory.Exists(arg) || File.Exists(arg)).ToList();

            var missingItems = args.Except(inputFoldersAndFiles);
            if (missingItems.Any())
            {
                Console.Error.WriteLine($"Unable to find the following file or directory: {String.Join(", ", missingItems)}");
            }
        }

        while (!inputFoldersAndFiles.Any())
        {
            Console.Clear();
            Console.WriteLine("Please provide a folder to merge the files:");
            var input = Console.ReadLine();
            if (Directory.Exists(input) || File.Exists(input))
            {
                inputFoldersAndFiles.Add(input);
            }
        }

        string outputFile = $"{Path.GetFileName(inputFoldersAndFiles.First())}-Merged.txt";
        if (File.Exists(outputFile))
        {
            File.Delete(outputFile);
        }

        try
        {
            using (var writer = new StreamWriter(outputFile))
            {
                var files = new List<string>();

                foreach (var inputFolderOrFile in inputFoldersAndFiles)
                {
                    if (Directory.Exists(inputFolderOrFile))
                    {
                        files.AddRange(SearchPatterns.SelectMany(pattern => Directory.EnumerateFiles(inputFolderOrFile, pattern, SearchOption.AllDirectories)));
                    }
                    else
                    {
                        files.Add(inputFolderOrFile);
                    }
                }

                var filteredFiles = files.Where(file => !ContainsAnyFolder(file, Excludeditems)).Distinct().ToList();

                foreach (string file in filteredFiles)
                {
                    var content = File.ReadAllText(file);
                    writer.WriteLine($"############## {file} ##############");
                    writer.WriteLine(content);
                }
            }

            Console.WriteLine($"Files have been merged into: {AppDomain.CurrentDomain.BaseDirectory}{outputFile}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred: {ex.Message}");
        }
    }

    private static bool ContainsAnyFolder(string filePath, string[] excludedFolders)
    {
        var parts = filePath.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
        return parts.Intersect(excludedFolders, StringComparer.OrdinalIgnoreCase).Any();
    }
}