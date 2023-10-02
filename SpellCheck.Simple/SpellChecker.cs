// Copyright (c) 2023 Krzysztof Prusik and contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using WeCantSpell.Hunspell;
using SpellCheck.Extensions;

namespace SpellCheck;

/// <summary>
/// The <see cref="SpellChecker"/> class provides functionality for spell-checking text,
/// including Markdown content, and managing dictionaries.
/// </summary>
public class SpellChecker : ISpellChecker
{
    private WordList wordList = WordList.CreateFromWords(new List<string>());

    private readonly QueryOptions queryOptions = new()
    {
        MaxSuggestions = 10,
        MaxCompoundSuggestions = 10,
        MaxGuess = 10,
        MaxPhoneticSuggestions = 10,
        MaxRoots = 10,
        MaxSharps = 10,
        MaxWords = 10,
        MinTimer = 10
    };

    /// <summary>
    /// Gets or sets the list of words that should be ignored during spell-checking.
    /// </summary>
    public List<string> IgnoredWords { get; set; } = new();

    /// <summary>
    /// Gets the count of words in the dictionary.
    /// </summary>
    public int DictionaryCount => wordList.RootWords.Count();

    /// <summary>
    /// Creates a new instance of the <see cref="SpellChecker"/> class with empty settings.
    /// </summary>
    public static SpellChecker Create()
    {
        var spellCheck = new SpellChecker();
        spellCheck.SetDictionary();
        spellCheck.SetIgnoredWords();
        return spellCheck;
    }
    
    /// <summary>
    /// Creates a new instance of the <see cref="SpellChecker"/> class using dictionary streams.
    /// </summary>
    public static async Task<SpellChecker> CreateFromStreams(Stream dictionary, Stream affix, 
        params string[] ignoredWords)
    {
        var spellCheck = Create();
        await spellCheck.SetDictionary(dictionary, affix);
        spellCheck.SetIgnoredWords(ignoredWords);
        return spellCheck;
    }
    
    /// <summary>
    /// Creates a new instance of the <see cref="SpellChecker"/> class using dictionary files.
    /// </summary>
    public static async Task<SpellChecker> CreateFromFiles(string dictionaryPath, string affixPath, 
        params string[] ignoredWords)
    {
        var spellCheck = Create();
        await spellCheck.SetDictionary(dictionaryPath, affixPath);
        spellCheck.SetIgnoredWords(ignoredWords);
        return spellCheck;        
    }

    /// <summary>
    /// Returns a list of suggested words for a given word.
    /// </summary>
    public IEnumerable<string> SuggestWord(string word) => 
        wordList.Suggest(word, queryOptions);

    /// <summary>
    /// Sets the list of ignored words for spell-checking.
    /// </summary>
    public void SetIgnoredWords(params string[] ignoredWords) => 
        IgnoredWords = ignoredWords.ToList();

    /// <summary>
    /// Sets the dictionary using streams for dictionary and affix files.
    /// </summary>
    public async Task SetDictionary(Stream dictionary, Stream affix) => 
        wordList = await WordList.CreateFromStreamsAsync(dictionary, affix);

    /// <summary>
    /// Sets the dictionary using file paths for dictionary and optional affix files.
    /// </summary>
    public async Task SetDictionary(string dictionaryPath, string? affixPath = null) =>
        wordList = affixPath is null
            ? await WordList.CreateFromFilesAsync(dictionaryPath)
            : await WordList.CreateFromFilesAsync(dictionaryPath, affixPath);

    /// <summary>
    /// Sets the dictionary using a list of words.
    /// </summary>
    public void SetDictionary(params string[] words) => 
        wordList = WordList.CreateFromWords(words);

    /// <summary>
    /// Checks if a word is spelled correctly.
    /// </summary>
    public bool IsWordCorrect(string word)
    {
        if (IgnoredWords.Contains(word))
            return true;
        
        var firstChar = word.Length > 0 ? word[0] : ' ';
        return string.IsNullOrWhiteSpace(word) || char.IsLetter(firstChar) is false || wordList.Check(word);
    }
    
    /// <summary>
    /// Checks if a text is spelled correctly.
    /// </summary>
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

    /// <summary>
    /// Checks the spelling of the provided text. Raise an <see cref="SpellCheckException"/>
    /// if an incorrect word is found.
    /// </summary>
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
        if (IsWordCorrect(word) is true) 
            return;
        
        throw new SpellCheckException(leaf, $"Incorrect word '{word}' in the text '{leaf}'.");
    }
}