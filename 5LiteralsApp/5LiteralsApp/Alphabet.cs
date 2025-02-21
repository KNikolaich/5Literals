namespace _5LiteralsApp;

internal record Alphabet
{
    /// <summary> Частота использования букв в алфавите </summary>
    internal static Dictionary<char, int> GetAlphabetFrequency() => new()
    {
        { 'о', 120 },
        { 'е', 110 },
        { 'а', 107 },
        { 'и', 102 },
        { 'н', 99 },
        { 'т', 97 },
        { 'с', 95 },
        { 'р', 90 },
        { 'в', 80 },
        { 'л', 75 },
        { 'к', 68 },
        { 'м', 64 },
        { 'д', 60 },
        { 'п', 57 },
        { 'у', 55 },
        { 'я', 45 },
        { 'ы', 40 },
        { 'ь', 37 },
        { 'г', 36 },
        { 'з', 34 },
        { 'б', 33 },
        { 'ч', 32 },
        { 'й', 30 },
        { 'х', 26 },
        { 'ж', 25 },
        { 'ш', 20 },
        { 'ю', 18 },
        { 'ц', 16 },
        { 'щ', 13 },
        { 'э', 12 },
        { 'ф', 10 },
        { 'ъ', 3 },
        //{ 'ё', 1 }
    };

    internal Alphabet(List<char> literals)
    {
        Literals = literals;
    }

    internal Alphabet() : this(GetAlphabetFrequency().Keys.ToList())
    {
    }

    internal List<char> Literals { get; set; }

    public override string ToString()
    {
        return $"Count: {Literals.Count}";
    }
}

internal static class AlphabetExtender
{
    /// <summary> Есть все буквы этого слова в алфавите </summary>
    internal static bool HasAllLiterals(this Alphabet alphabet, string word) 
        => alphabet.Literals.Count > word.Length && word.All(alphabet.Literals.Contains);

    /// <summary> Убрать все буквы из нашего набора </summary>
    internal static void RemoveLiterals(this Alphabet alphabet, WordEntity word)
    {
        foreach (var literal in word.Word)
        {
            alphabet.Literals.Remove(literal);
        }
    }
}