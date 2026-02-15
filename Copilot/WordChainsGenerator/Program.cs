using System.Globalization;

namespace WordChainsGenerator;

public class Program
{
    public static async Task<int> Main(string[] args)
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

        Console.WriteLine("Введите '-d <слово>' чтобы удалить слово из файла words.txt, либо нажмите Enter для выхода:");
        var input = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(input) && input.StartsWith("-d ", StringComparison.OrdinalIgnoreCase))
        {
            var target = input.Substring(3).Trim().ToLowerInvariant();
            if (string.IsNullOrEmpty(target))
            {
                Console.WriteLine("Не указано слово для удаления.");
                return 0;
            }

            // Read original file, remove all lines equal to the target (case-insensitive, trimmed)
            try
            {
                var originalLines = File.ReadAllLines(file);
                var kept = originalLines.Where(l => !string.Equals(l?.Trim().ToLowerInvariant(), target, StringComparison.OrdinalIgnoreCase)).ToList();
                if (kept.Count == originalLines.Length)
                {
                    Console.WriteLine($"Слово '{target}' не найдено в файле {file}.");
                    return 0;
                }

                File.WriteAllLines(file, kept);
                Console.WriteLine($"Слово '{target}' удалено из файла {file}.");
                Console.WriteLine("Перезапустите программу, чтобы увидеть обновлённые цепочки.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении слова: {ex.Message}");
            }
        }

        var lines = await File.ReadAllLinesAsync(file);
        var words = WordChainGenerator.PrepareWords(lines);

        var generator = new WordChainGenerator(words);
        var result = generator.FindBestChains(maxLen);

        Console.WriteLine($"Максимально покрыто букв: {result.BestMaskCount}/{WordChainGenerator.RussianAlphabetCount}");
        Console.WriteLine("Лучшие цепочки (отсортированы по весу букв в порядке убывания):\n");
        
        // Sort chains by weight descending
        var sortedChains = result.BestChains.OrderByDescending(c => c.Weight).ToList();

        for (int i = 0; i < sortedChains.Count; i++)
        {
            ChainInfo? chain = sortedChains[i];
            var chainStr = string.Join(", ", chain.Words);
            Console.WriteLine($"{i+1:n4}\t{chainStr} \tВес: {chain.Weight:F4}");            
        }

        // Interactive deletion from words file: user may type "-d <word>" to remove it from words.txt
        Console.WriteLine();

        return 0;
    }
}

