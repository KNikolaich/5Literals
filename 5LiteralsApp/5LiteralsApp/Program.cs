// See https://aka.ms/new-console-template for more information
using _5LiteralsApp;

var treeBuilder = new TreeBuilder(new SourceFileDict());
var tree = treeBuilder.BuildTree();

var maxUniqLiterals = tree.Max(x => x.UniqueLiteralsCount());
var maximumList = tree
    .Where(x => x.UniqueLiteralsCount() == maxUniqLiterals) // подберем из 5 и 6 слов выборки
    .Select(x=> x.ToString())
    .Order()
    .ToList();

foreach (var wordList in maximumList)
{
    Console.WriteLine(wordList);
}
