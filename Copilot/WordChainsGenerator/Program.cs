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
        Console.WriteLine("Лучшие цепочки (отсортированы по весу букв в порядке убывания):\n");
        
        // Sort chains by weight descending
        var sortedChains = result.BestChains.OrderByDescending(c => c.Weight).ToList();
        
        foreach (var chain in sortedChains)
        {
            var chainStr = string.Join(", ", chain.Words);
            Console.WriteLine($"{chainStr} \tВес: {chain.Weight:F4}");            
        }
        
        Console.ReadKey();
        return 0;
    }
}

