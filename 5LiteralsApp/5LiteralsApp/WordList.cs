namespace _5LiteralsApp;

internal class WordList : List<string>
{
    /// <summary> первоначальный </summary>
    /// <param name="word"></param>
    public WordList(string word)
    {
        Alphabet = new Alphabet();  // инициализируем его в конструкторе
        AddWord(word);
    }

    /// <summary> для клона </summary>
    private WordList(WordList wordList)
    {
        Alphabet = new Alphabet(wordList.Alphabet.Literals);
    }

    internal Alphabet Alphabet { get; set; }

    /// <summary> Количество уникальных букв </summary>
    internal int UniqueLiteralsCount() => this.SelectMany(x => x.ToCharArray()).Distinct().Count();

    /// <summary> клон </summary>
    internal WordList Clone() => new(this);

    public void AddWord(string word)
    {
        Add(word);
        Alphabet.MinusLiterals(word);
    }

    public override string ToString() => string.Join(", ", this);
}