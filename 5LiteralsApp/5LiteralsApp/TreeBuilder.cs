using System.Collections.Generic;

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
        var words = sourceDictionary.GetWords().ToList();
        // для каждого слова создаем новую коллекцию, чтобы независимо от предыдущих слов считать
        var collectionLists = words.Select(w=> new WordList(w)).ToList();
        
        return CreateCollectionList(words, collectionLists);
    }

    private static List<WordList> CreateCollectionList(IEnumerable<WordEntity> words, List<WordList> collectionLists)
    {
        var result = new List<WordList>();
        foreach (var word in words)
        {
            foreach (var wordList in collectionLists)
            {
                if (wordList.Alphabet.HasAllLiterals(word.Word))
                {
                    // создаем новый worldList, добавляем его в коллекцию и минусуем ему в алфавите буквы
                    var clone = wordList.Clone();
                    clone.AddWord(word);
                    if (clone.Count >= 4)
                    {
                        result.Add(clone);
                    }
                    
                    result.AddRange(CreateCollectionList(words, new List<WordList> { clone }));
                }
            }
        }

        return result;
    }
}