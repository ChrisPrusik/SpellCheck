using System.Globalization;
using Xunit;

namespace SpellCheck.Dictionaries.Tests;

public class BuiltinSpellDictionariesTests : IClassFixture<Fixture>
{
    private readonly SpellCheckFactory spellCheckFactory;
    public BuiltinSpellDictionariesTests(Fixture fixture)
    {
        spellCheckFactory = fixture.SpellCheckFactory;
    }
    
    [Theory]
    [InlineData("pl-PL")]
    [InlineData("en-US")]
    [InlineData("en-GB")]
    [InlineData("de-DE")]
    [InlineData("es-ES")]
    [InlineData("fr-FR")]
    [InlineData("it-IT")]
    [InlineData("pt-PT")]
    [InlineData("ar-SR", false)]
    public void ContainsDictionary(string language, bool expectedStatus = true)
    {
        if (expectedStatus)
            Assert.True(spellCheckFactory.Contains(language));
        else
            Assert.False(spellCheckFactory.Contains(language));
    }
    
    [Theory]
    [InlineData("pl-PL")]
    [InlineData("en-US")]
    [InlineData("en-GB")]
    [InlineData("de-DE")]
    [InlineData("es-ES")]
    [InlineData("fr-FR")]
    [InlineData("it-IT")]
    [InlineData("pt-PT")]
    [InlineData("ar-SR", false)]
    public void ContainsCultureDictionary(string language, bool expectedStatus = true)
    {
        var culture = new CultureInfo(language);
        Assert.NotNull(culture);
        
        if (expectedStatus)
            Assert.True(spellCheckFactory.Contains(culture));
        else
            Assert.False(spellCheckFactory.Contains(culture));
    }
}