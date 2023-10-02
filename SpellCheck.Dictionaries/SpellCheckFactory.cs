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

using System.Globalization;
using System.Text;
using SpellCheck.Dictionaries.Extensions;

namespace SpellCheck.Dictionaries;

/// <summary>
///   SpellCheck.Simple factory class.
/// </summary>
public class SpellCheckFactory : ISpellCheckFactory
{
    /// <summary>
    ///   Dictionary directory.
    /// </summary>
    public string? DictionaryDirectory { get; set; }
    
    /// <summary>
    ///   Creates a new instance of <see cref="SpellCheckFactory"/> class.
    /// </summary>
    public SpellCheckFactory(string? dictionaryDirectory = null)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        if (string.IsNullOrWhiteSpace(dictionaryDirectory))
            return;
        
        if (Directory.Exists(dictionaryDirectory))
            DictionaryDirectory = dictionaryDirectory;
        else
            throw new DirectoryNotFoundException($"Directory '{dictionaryDirectory}' does not exist.");
    }

    /// <summary>
    ///   Creates a new instance of <see cref="SpellChecker"/> class.
    /// </summary>
    public async Task<SpellChecker> CreateSpellChecker(string language, 
        params string[] ignoredWords)
    {
        var culture = new CultureInfo(language);
        if (culture is null)
            throw new Exception($"Unknown language '{language}'.");

        return await CreateSpellChecker(culture, ignoredWords);
    }

    /// <summary>
    ///   Creates a new instance of <see cref="SpellChecker"/> class.
    /// </summary>
    public async Task<SpellChecker> CreateSpellChecker(CultureInfo culture, 
        params string[] ignoredWords)
    {
        await using var dictionary = culture.GetSpellCheckStream(DictionaryTypes.Dictionary, DictionaryDirectory);
        if (dictionary is null)
            throw new Exception($"Dictionary for '{culture.Name}' not found.");

        await using var affix = culture.GetSpellCheckStream(DictionaryTypes.Affix, DictionaryDirectory);
        if (affix is null)
            throw new Exception($"Affix for '{culture.Name}' not found.");
        
        return await SpellChecker.CreateFromStreams(dictionary, affix, ignoredWords);
    }

    /// <summary>
    ///   Checks if the specified culture is supported.
    /// </summary>
    public bool Contains(CultureInfo culture) =>
        culture.SpellCheckExists(DictionaryTypes.Dictionary, DictionaryDirectory) &&
        culture.SpellCheckExists(DictionaryTypes.Affix, DictionaryDirectory);

    /// <summary>
    ///   Checks if the specified language is supported.
    /// </summary>
    public bool Contains(string language) => 
        Contains(new CultureInfo(language));
}