namespace MergeTextFiles;

class Program
{
    static void Main(string[] args)
    {
        //if (args.Length != 1)
        //{
        //    Console.WriteLine("Kérlek adj meg egy mappa elérési útvonalat az argumentumként!");
        //    return;
        //}

        //string inputFolder = args[0]; // A bemeneti mappa elérési útvonala
        string inputFolder = "C:\\Work\\Snake"; // A bemeneti mappa elérési útvonala
        string outputFile = $"{Path.GetFileName(inputFolder)}-Output.txt"; // A kimeneti fájl neve

        try
        {
            using (var writer = new StreamWriter(outputFile))
            {
                // Rekurzívan végigjárja a mappa összes fájlját és almappáját
                foreach (string file in Directory.EnumerateFiles(inputFolder, "*.*", SearchOption.AllDirectories))
                {
                    // Csak a szöveges fájlokat dolgozza fel
                    if (IsTextFile(file))
                    {
                        string content = File.ReadAllText(file);
                        writer.WriteLine(content);
                    }
                }
            }

            Console.WriteLine("A fájlok sikeresen összefűzve a következő fájlba: " + outputFile);
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Hiba történt: " + ex.Message);
            Console.ReadLine();
        }
    }

    // Ellenőrzi, hogy a fájl szöveges típusú-e
    private static bool IsTextFile(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLower();
        return extension switch
        {
            ".cs" //or
            //".txt" or ".cpp" or ".js" or
            //".c++" or ".asm" or ".csproj" or ".ts" or
            //".config" or ".html" or ".css" or ".sln"

            => true,

            _ => false,
        };
    }
}