// See https://aka.ms/new-console-template for more information

using _5LiteralsApp;

// построим деревья с нашими наборами
var trees = TreeBuilder.BuildTrees(new SourceFileDictionary());

//// понимаем какое максимальное покрытие буквами в наших наборах есть
//var maxUniqLiterals = trees.Max(x => x.WightAllWords());

// делаем выборку только тех наборов слов, которые максимально покрывают алфавит
var maximumList = trees
    //.Where(x=> x.WightAllWords() >= maxUniqLiterals)
    .OrderByDescending(x => x.WightAllWords())
    //.DistinctBy(x => x.WightAllWords())
    //.OrderByDescending(x => x.Count)
    //.ThenByDescending(x=>x.WightAllWords())
    .Take(64)
    ;

// выводим их в консольку
foreach (var wordList in maximumList)
{
    Console.WriteLine(wordList);
}