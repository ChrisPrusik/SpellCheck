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

namespace SpellCheck;

/// <summary>
/// The <see cref="SpellChecker"/> interface provides functionality for spell-checking text,
/// </summary>
public interface ISpellChecker
{
    /// <summary>
    /// Gets or sets the list of words that should be ignored during spell-checking.
    /// </summary>
    List<string> IgnoredWords { get; set; }
    
    /// <summary>
    /// Gets the count of words in the dictionary.
    /// </summary>
    int DictionaryCount { get; }
    
    /// <summary>
    /// Creates a new instance of the <see cref="SpellChecker"/> class with empty settings.
    /// </summary>
    bool IsWordCorrect(string word);
    
    /// <summary>
    /// Creates a new instance of the <see cref="SpellChecker"/> class using dictionary streams.
    /// </summary>
    void CheckText(string text);
    
    /// <summary>
    /// Creates a new instance of the <see cref="SpellChecker"/> class using dictionary files.
    /// </summary>
    bool IsTextCorrect(string text);
    
    /// <summary>
    /// Returns a list of suggested words for a given word.
    /// </summary>
    IEnumerable<string> SuggestWord(string word);
    
    /// <summary>
    /// Sets the list of words that should be ignored during spell-checking.
    /// </summary>
    void SetIgnoredWords(params string[] ignoredWords);
    
    /// <summary>
    /// Sets the dictionary to be used for spell-checking.
    /// </summary>
    void SetDictionary(params string[] words);
    
    /// <summary>
    /// Sets the dictionary to be used for spell-checking.
    /// </summary>
    Task SetDictionary(string dictionaryPath, string? affixPath = null);
    
    /// <summary>
    /// Sets the dictionary to be used for spell-checking.
    /// </summary>
    Task SetDictionary(Stream dictionary, Stream affix);
}