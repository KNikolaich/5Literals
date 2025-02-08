namespace _5LiteralsApp;

internal record SourceFileDict : ISourceDictionary
{

    public IEnumerable<string> GetWords()
    {
        if (File.Exists("russian.txt"))
        {
            string[] lines = File.ReadAllLines("russian5.txt");
            foreach (string s in lines
                         .Select(x => x.Trim(' ', '_'))
                         .Where(x => x.Length == 5 && !x.Contains('-') && x.Distinct().Count() >= 4))
            {
                yield return s;
            }
        }
    }
}