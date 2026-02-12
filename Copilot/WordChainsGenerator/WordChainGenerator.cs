using System.Text;
using System.Linq;

namespace WordChainsGenerator;

public record ChainInfo(List<string> Words, int LetterCount, double Weight);

public record FindResult(int BestMaskCount, List<ChainInfo> BestChains);

public class WordChainGenerator
{
    // Russian alphabet including ё (33 letters)
    public static readonly char[] RussianAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя".ToCharArray();
    public static readonly int RussianAlphabetCount = RussianAlphabet.Length;
    private static readonly Dictionary<char, int> _letterToIndex;
    
    // Letter frequency weights in Russian (normalized to 0-1 range)
    private static readonly double[] _letterWeights = new double[33]
    {
        0.0800,  // а
        0.0159,  // б
        0.0453,  // в
        0.0169,  // г
        0.0298,  // д
        0.0849,  // е
        0.0030,  // ё
        0.0094,  // ж
        0.0164,  // з
        0.0737,  // и
        0.0121,  // й
        0.0349,  // к
        0.0434,  // л
        0.0320,  // м
        0.0670,  // н
        0.0110,  // о
        0.0280,  // п
        0.0475,  // р
        0.0547,  // с
        0.0632,  // т
        0.0262,  // у
        0.0027,  // ф
        0.0097,  // х
        0.0049,  // ц
        0.0145,  // ч
        0.0072,  // ш
        0.0036,  // щ
        0.0004,  // ъ
        0.0190,  // ы
        0.0174,  // ь
        0.0033,  // э
        0.0064,  // ю
        0.0200   // я
    };

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
    
    // Calculate weight of mask (sum of letter frequencies)
    private double CalculateWeight(int mask)
    {
        double weight = 0.0;
        for (int i = 0; i < 33; i++)
        {
            if ((mask & (1 << i)) != 0)
                weight += _letterWeights[i];
        }
        return weight;
    }

    public FindResult FindBestChains(int? maxChainLength = null)
    {
        int bestCount = 0;
        var bestChains = new List<ChainInfo>();

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
                var weight = CalculateWeight(currentMask);
                bestChains.Add(new ChainInfo(new List<string>(chain), currentCount, weight));
            }
            else if (currentCount == bestCount && currentCount > 0)
            {
                var weight = CalculateWeight(currentMask);
                bestChains.Add(new ChainInfo(new List<string>(chain), currentCount, weight));
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

