using System.Diagnostics.CodeAnalysis;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using WeCantSpell.Hunspell;
using SpellCheck.Extensions;

namespace SpellCheck;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class SpellCheck : ISpellCheck
{
    private WordList wordList = WordList.CreateFromWords(new List<string>());

    public List<string> IgnoredWords { get; set; } = new();

    public int DictionaryCount => wordList.RootWords.Count();

    public static SpellCheck Create()
    {
        var spellCheck = new SpellCheck();
        spellCheck.SetDictionary();
        spellCheck.SetIgnoredWords();
        return spellCheck;
    }
    
    public static async Task<SpellCheck> CreateFromStreams(Stream dictionary, Stream affix, 
        params string[] ignoredWords)
    {
        var spellCheck = Create();
        await spellCheck.SetDictionary(dictionary, affix);
        spellCheck.SetIgnoredWords(ignoredWords);
        return spellCheck;
    }
    
    public static async Task<SpellCheck> CreateFromFiles(string dictionaryPath, string affixPath, 
        params string[] ignoredWords)
    {
        var spellCheck = Create();
        await spellCheck.SetDictionary(dictionaryPath, affixPath);
        spellCheck.SetIgnoredWords(ignoredWords);
        return spellCheck;        
    }

    public IEnumerable<string> SuggestWord(string word)
    {
        return wordList.Suggest(word);
    }

    public void SetIgnoredWords(params string[] ignoredWords)
    {
        IgnoredWords = ignoredWords.ToList();
    }
    
    public async Task SetDictionary(Stream dictionary, Stream affix)
    {
        wordList = await WordList.CreateFromStreamsAsync(dictionary, affix);
    }
    
    public async Task SetDictionary(string dictionaryPath, string? affixPath = null)
    {
        if (affixPath is null)
            wordList = await WordList.CreateFromFilesAsync(dictionaryPath);
        else 
            wordList = await WordList.CreateFromFilesAsync(dictionaryPath, affixPath);
    }

    public void SetDictionary(params string[] words)
    {
        wordList = WordList.CreateFromWords(words);
    }

    public bool IsWordCorrect(string word)
    {
        if (IgnoredWords.Contains(word))
            return true;
        
        var firstChar = word.Length > 0 ? word[0] : ' ';
        return string.IsNullOrWhiteSpace(word) || char.IsLetter(firstChar) is false || wordList.Check(word);
    }
    
    public bool IsTextCorrect(string text)
    {
        try
        {
            CheckText(text);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void CheckText(string text)
    {
        var document = text.GetMarkdownDocument();

        foreach (var leaf in document.Descendants<LeafInline>())
        {
            if (leaf.Span.Length <= 0)
                continue;

            CheckWordsInLeaf(text, leaf);
        }
    }

    private void CheckWordsInLeaf(string text, LeafInline leaf)
    {
        foreach (var word in text.GetSubstring(leaf).GetWords()) 
            CheckWord(leaf, word);
    }

    private void CheckWord(LeafInline leaf, string word)
    {
        if (IsWordCorrect(word) is false)
            throw new SpellCheckException(leaf, $"Incorrect word '{word}' in the text '{leaf}'.");
    }
}