namespace _5LiteralsApp;

internal record SourceFileDictionary : ISourceDictionary
{
    public IEnumerable<string> GetWords()
    {
        var fileName = "russian4.txt";
        if (File.Exists(fileName))
        {
            string[] lines = File.ReadAllLines(fileName);
            foreach (string s in lines
                         .Select(x => x.Trim(' ', '_'))
                         .Where(x => x.Length == 5 && !x.Contains('-') && x.Distinct().Count() == 5))
            {
                yield return s.ToLower();
            }
        }
    }
}