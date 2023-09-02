using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using Xunit;
using SpellCheck;

namespace SpellCheck.Tests;

public class ComplexSpellCheckTests : IClassFixture<Fixture>
{
    private readonly Dictionary<string, SpellCheck> spellCheckers;
    public ComplexSpellCheckTests(Fixture fixture)
    {
        spellCheckers = fixture.SpellCheckers;
    }

    [Theory]
    [InlineData("en", "test-en-us.txt")]
    [InlineData("pl", "test-pl-pl.txt")]
    [InlineData("en", "markdown-en-us.md")]
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
            throw new Exception($"SpellCheck should throw an exception for the text '{fileName}'.");
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