namespace _5LiteralsApp;

internal class TreeBuilder
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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceDictionary"></param>
    /// <returns></returns>
    internal static ICollection<WordList> BuildTrees(ISourceDictionary sourceDictionary)
    {
        var collectionLists = new HashSet<WordList>();
        var pastWordList = new List<string>();
        foreach (string wordSource in sourceDictionary.GetWords())
        {
            var word = wordSource.Trim();
            // для каждого слова создаем новую коллекцию, чтобы независимо от предыдущих слов считать

            var currentWordList = new WordList(word);
            collectionLists.Add(currentWordList);

            foreach (WordList wordList in collectionLists.ToArray())
            {
                CheckWord(wordList, word, collectionLists);
            }
            pastWordList.ForEach(x=> CheckWord(currentWordList, x, collectionLists));
            pastWordList.Add(word);
        }
        return collectionLists;
    }

    private static void CheckWord(WordList wordList, string word, HashSet<WordList> collectionLists)
    {
        if (wordList.Alphabet.HasAllLiterals(word) || wordList.Count == 5)
        {
            wordList.AddWord(word);

            // создаем новый worldList, добавляем его в коллекцию и минусуем ему в алфавите буквы
            var cloneWordList = wordList.Clone();

            collectionLists.Add(cloneWordList);
        }
    }
}