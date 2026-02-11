using System.Globalization;

namespace WordChainsGenerator;

class Program
{
    static async Task<int> Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var file = args.Length > 0 ? args[0] : "words.txt";
        int? maxLen = null;
        if (args.Length > 1 && int.TryParse(args[1], out var v)) maxLen = v;

        if (!File.Exists(file))
        {
            Console.WriteLine($"Файл со словами не найден: {file}");
            return 1;
        }

        var lines = await File.ReadAllLinesAsync(file);
        var words = WordChainGenerator.PrepareWords(lines);

        var generator = new WordChainGenerator(words);
        var result = generator.FindBestChains(maxLen);

        Console.WriteLine($"Максимально покрыто букв: {result.BestMaskCount}/{WordChainGenerator.RussianAlphabetCount}");
        Console.WriteLine("Лучшие цепочки (каждая цепочка — слова через запятую):");
        foreach (var chain in result.BestChains)
        {
            Console.WriteLine(string.Join(", ", chain));
        }
        Console.ReadKey();
        return 0;
    }
}
