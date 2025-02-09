namespace _5LiteralsApp;

internal class WordList : List<string>
{
    /// <summary> первоначальный </summary>
    /// <param name="word">добавляем сразу слово</param>
    public WordList(string word)
    {
        Alphabet = new Alphabet();  // инициализируем его в конструкторе
        AddWord(word);
    }

    /// <summary> для создания клона </summary>
    private WordList(WordList wordList)
    {
        Alphabet = new Alphabet(wordList.Alphabet.Literals);
    }

    internal Alphabet Alphabet { get; set; }

    /// <summary> клон </summary>
    internal WordList Clone() => new(this);

    /// <summary> Количество уникальных букв </summary>
    internal int UniqueLiteralsCount() 
        => this.SelectMany(x => x.ToCharArray()).Distinct().Count();

    public void AddWord(string word)
    {
        Add(word);
        Alphabet.RemoveLiterals(word);
    }

    /// <summary> Для простоты отображения </summary>
    public override string ToString() 
        => string.Join(", ", this);
}