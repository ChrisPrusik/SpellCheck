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

using Xunit;

namespace SpellCheck.Tests;

public class SimpleSpellCheckTests : IClassFixture<Fixture>
{
    private const string badWord = "omanopatea";

    private readonly Dictionary<string, SpellCheck> spellCheckers;
    public SimpleSpellCheckTests(Fixture fixture)
    {
        spellCheckers = fixture.SpellCheckers;
    }

    [Theory]
    [InlineData("en_US.dic", "en_US.aff", "hello", "world")]
    [InlineData("pl_PL.dic", "pl_PL.aff", "część", "świecie")]
    public async Task SetDictionaryFiles(string dictionaryPath, string affixPath, params string[] words)
    {
        var spellCheck = new SpellCheck();
        Assert.NotNull(spellCheck);
       
        await spellCheck.SetDictionary(dictionaryPath, affixPath);
        Assert.True(spellCheck.DictionaryCount > 0);
        foreach (var word in words)
            Assert.True(spellCheck.IsWordCorrect(word));
        
        Assert.False(spellCheck.IsWordCorrect(badWord));
    }

    [Theory]
    [InlineData("en_US.dic", "en_US.aff", "hello", "world")]
    [InlineData("pl_PL.dic", "pl_PL.aff", "część", "świecie")]
    public async Task SetDictionaryStreams(string dictionaryPath, string affixPath, params string[] words)
    {
        var spellCheck = new SpellCheck();
        Assert.NotNull(spellCheck);

        await using var dictionary = File.OpenRead(dictionaryPath);
        await using var affix = File.OpenRead(affixPath);
        await spellCheck.SetDictionary(dictionary, affix);
        Assert.True(spellCheck.DictionaryCount > 0);
        foreach (var word in words)
            Assert.True(spellCheck.IsWordCorrect(word));
        
        Assert.False(spellCheck.IsWordCorrect(badWord));
    }

    [Theory]
    [InlineData("hello", "world")]
    [InlineData("część", "świecie")]
    public void SetIgnoredWords(params string[] words)
    {
        var spellCheck = new SpellCheck();
        Assert.NotNull(spellCheck);
        Assert.Equal(0, spellCheck.DictionaryCount);

        foreach (var word in words)
            Assert.False(spellCheck.IsWordCorrect(word));
        
        spellCheck.SetIgnoredWords(words);
        Assert.Equal(words.Length, spellCheck.IgnoredWords.Count);

        foreach (var word in words)
            Assert.True(spellCheck.IsWordCorrect(word));
        
        Assert.False(spellCheck.IsWordCorrect(badWord));
    }

    [Theory]
    [InlineData("Hello", "World")]
    [InlineData("witaj", "świecie")]
    public void SetDictionary(params string[] words)
    {
        var spellCheck = new SpellCheck();
        spellCheck.SetDictionary(words);
        foreach (var word in words)
            Assert.True(spellCheck.IsWordCorrect(word));
        
        Assert.False(spellCheck.IsWordCorrect(badWord));
    }
    
    [Theory]
    [InlineData("en", "Hello", true)]
    [InlineData("en", "World", true)]
    [InlineData("en", "Helo", false)]
    [InlineData("en", "Worl", false)]
    [InlineData("en", "Hello world!", false)]
    [InlineData("en", "world!", false)]
    [InlineData("en", "!", true)]
    public void CheckWord(string language, string word, bool expectedStatus)
    {
        if (spellCheckers.ContainsKey(language) is false)
            throw new Exception($"Unknown language '{language}' in the InlineData().");
                
        if (expectedStatus)
            Assert.True(spellCheckers[language].IsWordCorrect(word));
        else
            Assert.False(spellCheckers[language].IsWordCorrect(word));
    }

    [Theory]
    [InlineData("en", ".", true)]
    [InlineData("en", "Hello.", true)]
    [InlineData("en", "World.", true)]
    [InlineData("en", "hello", false)]
    [InlineData("en", "Helo.", false)]
    [InlineData("en", "Worl.", false)]
    [InlineData("en", "Hello world!", true)]
    [InlineData("en", "world!", false)]
    public void CheckText(string language, string paragraph, bool expectedStatus)
    {
        if (spellCheckers.ContainsKey(language) is false)
            throw new Exception($"Unknown language '{language}' in the InlineData().");

        if (expectedStatus)
                spellCheckers[language].CheckText(paragraph);
        else
            try
            {
                spellCheckers[language].CheckText(paragraph);
                throw new Exception("Exception was not thrown.");
            }
            catch
            {
            }
    }
}