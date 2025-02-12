namespace _5LiteralsApp;

internal class WordList : List<WordEntity>
{
    /// <summary> первоначальный </summary>
    /// <param name="word">добавляем сразу слово</param>
    public WordList(WordEntity word)
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
    internal int WightAllWords()
    {
        return this.SelectMany(x => x.Word.ToCharArray()).Sum(x => Alphabet.GetAlphabetFrequency()[x]);
    }

    public void AddWord(WordEntity word)
    {
        Add(word);
        Alphabet.RemoveLiterals(word);
    }

    /// <summary> Для простоты отображения </summary>
    public override string ToString()
    {
        return string.Join(", ", this.OrderByDescending(x=> x.Wight)) 
               + " Count:" 
               + this.SelectMany(x => x.Word.ToCharArray()).Distinct().Count();
    }
}

internal record WordEntity
{
    public WordEntity(string word)
    {
        Word = word;
        Wight = word.ToCharArray().Sum(x => Alphabet.GetAlphabetFrequency()[x]);
    }
    
    public string Word { get; }

    public int Wight { get; }

    public override string ToString() => Word;
}