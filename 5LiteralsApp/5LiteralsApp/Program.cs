// See https://aka.ms/new-console-template for more information
using _5LiteralsApp;

var treeBuilder = new TreeBuilder(new SourceMemoryDict());
var tree = treeBuilder.BuildTree();
var maxUniq = tree.Max(w => w.UniqueLiteralsCount());
var maximumList = tree
    .Where(x => x.UniqueLiteralsCount() == maxUniq)
    .Select(x=>x.ToString())
    .Order()
    .ToList();

foreach (var wordList in maximumList)
{
    Console.WriteLine(wordList);
}
