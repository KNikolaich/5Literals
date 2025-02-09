namespace _5LiteralsApp;

internal record Alphabet
{
    internal Alphabet(List<char> literals)
    {
        Literals = literals;
    }

    internal Alphabet() : this("абвгдеёжзийклмнопрстуфхцчшщъыьэюя".ToList())
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
    internal static void RemoveLiterals(this Alphabet alphabet, string word)
    {
        foreach (var literal in word)
        {
            alphabet.Literals.Remove(literal);
        }
    }
}