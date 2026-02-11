using Xunit;
using WordChainsGenerator;

namespace WordChainsGenerator.Tests;

public class WordChainGeneratorTests
{
    [Fact]
    public void SimpleHappyPath_FindsNonEmptyChain()
    {
        var words = new[] { "абв", "где", "жз" };
        var gen = new WordChainGenerator(words);
        var res = gen.FindBestChains();
        Assert.Equal(8, res.BestMaskCount); // all 8 letters unique across the three words combined
    }

    [Fact]
    public void DuplicateLettersInWord_AreIgnored()
    {
        var words = new[] { "мама", "кот" }; // "мама" has repeated 'м' and 'а' so it should be ignored
        var gen = new WordChainGenerator(words);
        var res = gen.FindBestChains();
        // only "кот" remains with 3 letters
        Assert.Equal(3, res.BestMaskCount);
    }
}
