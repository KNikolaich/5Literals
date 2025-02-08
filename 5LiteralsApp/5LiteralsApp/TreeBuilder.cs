namespace _5LiteralsApp;

internal class TreeBuilder(ISourceDictionary sourceDictionary)
{
    /*
     * Берем слово, создаем набор слов с алфавитом (если первое, иначе, клонируем с исходника)
     * перебираем все наборы слов, имеющиеся в словаре
     * если букв в алфавите меньше (либо =) 5, переносим из словаря обрабатываемых в словарь конечных
     * проверяем, что слово складывается из букв алфавита
     * да
     *      добавляем в рабор слов это слово,
     *      выкидываем из алфавита буквы этого слова
     *
     * 
     * если есть ещё слова, ->
     * Берем следуеюее слово и идем в начало алгоритма     
     */

    ICollection<string> _source = new List<string>(); // 

    ICollection<WordList> _collectionLists = new HashSet<WordList>();

    internal ICollection<WordList> BuildTree()
    {
        _source = sourceDictionary.GetWords();
        
        foreach (string wordSource in _source)
        {
            var word = wordSource.Trim();
            // для каждого слова создаем новую коллекцию, чтобы независимо от предыдущих слов считать

            _collectionLists.Add(new WordList(word));

            foreach (WordList wordList in _collectionLists.ToArray())
            {
                if (wordList.Alphabet.HasAllLiterals(word))
                {
                    wordList.AddWord(word);

                    // создаем новый worldList, добавляем его в коллекцию и минусуем ему в алфавите буквы
                    var cloneWordList = wordList.Clone();

                    _collectionLists.Add(cloneWordList);
                }
            }
        }
        return _collectionLists;
    }
}