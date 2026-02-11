using System.Text;
using System.Linq;

namespace WordChainsGenerator;

public record FindResult(int BestMaskCount, List<List<string>> BestChains);

public class WordChainGenerator
{
    // Russian alphabet including ё (33 letters)
    public static readonly char[] RussianAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя".ToCharArray();
    public static readonly int RussianAlphabetCount = RussianAlphabet.Length;
    private static readonly Dictionary<char, int> _letterToIndex;

    static WordChainGenerator()
    {
        _letterToIndex = new Dictionary<char, int>();
        for (int i = 0; i < RussianAlphabet.Length; i++) _letterToIndex[RussianAlphabet[i]] = i;
    }

    private readonly List<(string Word, int Mask)> _words;

    public WordChainGenerator(IEnumerable<string> words)
    {
        _words = words.Select(w => (Word: w, Mask: MaskOf(w))).Where(t => t.Mask != 0).ToList();
        // sort by letter count descending to improve pruning
        _words.Sort((a, b) => CountBits(b.Mask).CompareTo(CountBits(a.Mask)));
    }

    // Cleans and prepares a list of words from raw lines
    public static IEnumerable<string> PrepareWords(IEnumerable<string> lines)
    {
        foreach (var raw in lines)
        {
            if (string.IsNullOrWhiteSpace(raw)) continue;
            var cleaned = raw.Trim().ToLowerInvariant();
            yield return cleaned;
        }
    }

    // Convert a word to bitmask; returns 0 if invalid (contains non-russian letters or duplicated letters)
    public static int MaskOf(string word)
    {
        int mask = 0;
        foreach (var ch in word)
        {
            if (!_letterToIndex.TryGetValue(ch, out var idx)) return 0; // invalid char
            int bit = 1 << idx;
            if ((mask & bit) != 0) return 0; // repeated letter inside the word
            mask |= bit;
        }
        return mask;
    }

    public FindResult FindBestChains(int? maxChainLength = null)
    {
        int bestCount = 0;
        var bestChains = new List<List<string>>();

        int n = _words.Count;
        int[] suffixUnion = new int[n + 1];
        // precompute union of masks from i..end
        for (int i = n - 1; i >= 0; i--) suffixUnion[i] = suffixUnion[i + 1] | _words[i].Mask;

        void Dfs(int start, int currentMask, List<string> chain)
        {
            int currentCount = CountBits(currentMask);
            // update best
            if (currentCount > bestCount)
            {
                bestCount = currentCount;
                bestChains.Clear();
                bestChains.Add(new List<string>(chain));
            }
            else if (currentCount == bestCount && currentCount > 0)
            {
                bestChains.Add(new List<string>(chain));
            }

            if (maxChainLength.HasValue && chain.Count >= maxChainLength.Value) return;

            for (int i = start; i < n; i++)
            {
                // pruning: if union of remaining cannot improve bestCount, break
                int possible = CountBits(currentMask | suffixUnion[i]);
                if (possible <= bestCount) break;

                var (word, mask) = _words[i];
                if ((currentMask & mask) != 0) continue; // conflict letters

                chain.Add(word);
                Dfs(i + 1, currentMask | mask, chain);
                chain.RemoveAt(chain.Count - 1);
            }
        }

        Dfs(0, 0, new List<string>());

        return new FindResult(bestCount, bestChains);
    }

    private static int CountBits(int v) => System.Numerics.BitOperations.PopCount((uint)v);
}
