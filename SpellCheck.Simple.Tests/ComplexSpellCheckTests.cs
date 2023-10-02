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

namespace SpellCheck.Simple.Tests;

public class ComplexSpellCheckTests : IClassFixture<Fixture>
{
    private readonly Dictionary<string, SpellChecker> spellCheckers;
    public ComplexSpellCheckTests(Fixture fixture)
    {
        spellCheckers = fixture.SpellCheckers;
    }

    [Theory]
    [InlineData("en", "markdown-en-us.md")]
    [InlineData("en", "test-en-us.txt")]
    [InlineData("pl", "test-pl-pl.txt")]
    public void CheckTextPositive(string language, string fileName)
    {
        if (spellCheckers.ContainsKey(language) is false)
            throw new Exception($"Unknown language '{language}' in the InlineData().");
        
        var text = GetTextFromFile(fileName);
        spellCheckers[language].CheckText(text);
    }  

    [Theory]
    [InlineData("en", "test-en-us.txt", "liability", "liabbbility")]
    [InlineData("pl", "test-pl-pl.txt", "Użytkownicy", "Użyszkownicy")]
    public void CheckTextNegative(string language, string fileName, string current, string replaced)
    {
        if (spellCheckers.ContainsKey(language) is false)
            throw new Exception($"Unknown language '{language}' in the InlineData().");
        
        var text = GetTextFromFile(fileName).Replace(current, replaced);
        try
        {
            spellCheckers[language].CheckText(text);
            throw new Exception($"SpellCheck.Simple should throw an exception for the text '{fileName}'.");
        }
        catch (SpellCheckException e)
        {
            Assert.NotNull(e);
        }
    } 
    
    private string GetTextFromFile(string fileName)
    {
        Assert.True(Directory.Exists("Tests"));
        var path = Path.Combine("Tests", fileName);
        Assert.True(File.Exists(path));
        return File.ReadAllText(path);
    }
}   