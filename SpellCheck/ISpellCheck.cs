namespace SpellCheck;

public interface ISpellCheck
{
    List<string> IgnoredWords { get; set; }
    int DictionaryCount { get; }
    bool IsWordCorrect(string word);
    void CheckText(string text);
    bool IsTextCorrect(string text);
    IEnumerable<string> SuggestWord(string word);
    void SetIgnoredWords(params string[] ignoredWords);
    void SetDictionary(params string[] words);
    Task SetDictionary(string dictionaryPath, string? affixPath = null);
    Task SetDictionary(Stream dictionary, Stream affix);
}