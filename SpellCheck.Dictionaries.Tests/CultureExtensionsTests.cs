using System.Globalization;
using SpellCheck.Dictionaries.Extensions;
using Xunit;

namespace SpellCheck.Dictionaries.Tests;

public class CultureExtensionsTests
{
    [Theory]
    [InlineData("ar-SR", DictionaryTypes.Dictionary)]
    [InlineData("ar-SR", DictionaryTypes.Affix)]
    [InlineData("pl-PL", DictionaryTypes.Dictionary, false)]
    [InlineData("pl-PL", DictionaryTypes.Affix, false)]
    [InlineData("ru-RU", DictionaryTypes.Dictionary, true, "Test")]
    [InlineData("ru-RU", DictionaryTypes.Affix, true, "Test")]
    [InlineData("en-GB", DictionaryTypes.Dictionary, false, "Test")]
    [InlineData("en-GB", DictionaryTypes.Affix, false, "Test")]
    public void GetSpellCheckFileStream(string language, DictionaryTypes type, bool expectedStatus = true, string? directory = null)
    {
        var culture = new CultureInfo(language);
        Assert.NotNull(culture);
        
        var stream = culture.GetSpellCheckFileStream(type, directory);
        if (expectedStatus)
            Assert.NotNull(stream);
        else
            Assert.Null(stream);
    }
    
    [Theory]
    [InlineData("pl-PL", DictionaryTypes.Dictionary)]
    [InlineData("pl-PL", DictionaryTypes.Affix)]
    [InlineData("ar-SR", DictionaryTypes.Dictionary, false)]
    [InlineData("ar-SR", DictionaryTypes.Affix, false)]
    public void GetSpellCheckResourceStream(string language, DictionaryTypes type, bool expectedStatus = true)
    {
        var culture = new CultureInfo(language);
        Assert.NotNull(culture);
        
        var stream = culture.GetSpellCheckResourceStream(type);
        if (expectedStatus)
            Assert.NotNull(stream);
        else
            Assert.Null(stream);
    }
    
    [Theory]
    [InlineData("pl-PL", DictionaryTypes.Dictionary)]
    [InlineData("pl-PL", DictionaryTypes.Affix)]
    [InlineData("ar-SR", DictionaryTypes.Dictionary)]
    [InlineData("ar-SR", DictionaryTypes.Affix)]
    [InlineData("ru-RU", DictionaryTypes.Dictionary, false)]
    [InlineData("ru-RU", DictionaryTypes.Affix, false)]
    public void GetSpellCheckStream(string language, DictionaryTypes type, bool expectedStatus = true)
    {
        var culture = new CultureInfo(language);
        Assert.NotNull(culture);
        
        var stream = culture.GetSpellCheckStream(type);
        if (expectedStatus)
            Assert.NotNull(stream);
        else
            Assert.Null(stream);
    }
}