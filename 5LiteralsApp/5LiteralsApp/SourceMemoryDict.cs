namespace _5LiteralsApp;

internal interface ISourceDictionary
{
    ICollection<string> GetWords();
}

internal record SourceMemoryDict : ISourceDictionary
{
    ICollection<string> ISourceDictionary.GetWords() =>
        new List<string>
        {
            "кобра",
            "вздор",
            "вздох",
            "багет",
            "сквер",
            "шпиль",
            "кумыс",
            "глубь",
            "кобра",
            "якорь",
            "вышка",
            "салют",
            "егоза",
            "связь",
            "цинга",
            "мечта",
            "муляж",
            "шепот",
            "крыса",
            "ферзь",
            "купон",
            "миска",
            "выпад",
            "гладь",
            "зыбун",
            "факел",
            "хвост",
            "буква",
            "пенис",
            "лютин",
            "жетон",
            "бетон",
            "угорь",
            "бочка",
            "ямщик",
            "бизон",
            "бидон",
            "щегол",
            "жизнь",
            "хруст",
            "тикер"
        };
}