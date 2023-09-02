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
        var document = Markdown.Parse(text);

        foreach (var block in document.Descendants().Where(x => x is Block))
        {
            if (block is ParagraphBlock paragraph)
                CheckParagraphBlock(paragraph, text);
            else if (block is HeadingBlock heading)
                CheckHeadingBlock(heading, text);
        }
    }

    private void CheckHeadingBlock(HeadingBlock heading, string text)
    {
        CheckWordsInBlock(heading, text);
    }

    private void CheckParagraphBlock(ParagraphBlock block, string text)
    {
        CheckWordsInBlock(block, text);
    }

    private void CheckWordsInBlock(Block block, string text)
    {
        var checkText = GetTextFromBlock(block, text);
        foreach (var word in checkText.GetWords())
            if (IsWordCorrect(word) is false)
                throw new SpellCheckException(block, $"Bad word '{word}' in the paragraph '{checkText}'.");
    }
    
    private string GetMarkdownText(MarkdownObject item, string text) => 
        text.Substring(item.Span.Start, item.Span.Length);

    private string GetTextFromBlock(Block block, string text)
    {
        // return text.Substring((MarkdownObject)block);
        var result = GetMarkdownText(block, text);
        return result;
    }
    
    private string RemoveUncheckedInlines(Inline inline, string text)
    {
        if (inline is LinkInline or AutolinkInline or CodeInline or HtmlEntityInline or HtmlInline)
            return RemoveUnusedInline(inline, text); 
        
        if (inline is LiteralInline)
            return text.Remove(inline.Span.Start, inline.Span.Length).Insert(inline.Span.Start, new string(' ', inline.Span.Length));

        return text;
    }

    private string RemoveUnusedInline(Inline inline, string text) => 
        text.Remove(inline.Span.Start, inline.Span.Length).Insert(inline.Span.Start, new string(' ', inline.Span.Length));

}