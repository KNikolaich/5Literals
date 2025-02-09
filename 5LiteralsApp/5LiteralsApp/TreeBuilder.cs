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
    internal static ICollection<WordList> BuildTrees(ISourceDictionary sourceDictionary)
    {
        var collectionLists = new HashSet<WordList>();
        foreach (string wordSource in sourceDictionary.GetWords())
        {
            var word = wordSource.Trim();
            // для каждого слова создаем новую коллекцию, чтобы независимо от предыдущих слов считать

            collectionLists.Add(new WordList(word));

            foreach (WordList wordList in collectionLists.ToArray())
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
        return collectionLists;
    }
}