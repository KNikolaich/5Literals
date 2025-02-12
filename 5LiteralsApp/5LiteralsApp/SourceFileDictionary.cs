namespace _5LiteralsApp;

internal record SourceFileDictionary : ISourceDictionary
{
    public IEnumerable<WordEntity> GetWords()
    {
        return File.ReadAllLines("russian4.txt")
            .Select(x => x.Trim(' ', '_'))
            .Where(x => x.Length == 5 && !x.Contains('-') && x.Distinct().Count() == 5)
            .Select(w => new WordEntity(w.ToLower()))
            .OrderBy(x => x.Wight)
            ;
    }
}