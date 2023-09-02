using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace SpellCheck.Dictionaries.Tests;

public class FileSpellDictionariesTests : IClassFixture<Fixture>
{
    private readonly Fixture fixture;
    
    public FileSpellDictionariesTests(Fixture fixture)
    {
        this.fixture = fixture;
    }

    [Theory]
    [InlineData("ar-SR", true, "اكتشف السعودية بطرق متنوعة")]
    [InlineData("en-GB", false)]
    public async Task CheckFileDictionary(string language, bool expectedStatus = true, string? textToCheck = null)
    {
        if (expectedStatus)
        {
            await fixture.CheckLanguageDictionary(language, textToCheck);
            Directory.CreateDirectory("Dictionaries");
            File.Move("ar_SR.dic", "Dictionaries/ar_SR.dic", true);
            File.Move("ar_SR.aff", "Dictionaries/ar_SR.aff", true);
            Assert.False(fixture.SpellCheckFactory.Contains(language));
            fixture.SpellCheckFactory.DictionaryDirectory = "Dictionaries";
            Assert.True(fixture.SpellCheckFactory.Contains(language));
            try
            {
                await fixture.CheckLanguageDictionary(language, textToCheck);
            }
            catch
            {
                fixture.SpellCheckFactory.DictionaryDirectory = string.Empty;
                throw;
            }
        }
        else
            Assert.False(fixture.SpellCheckFactory.Contains(language));
    }

    [Theory]
    [InlineData("ar-SR", true, "اكتشف السعودية بطرق متنوعة")]
    [InlineData("en-GB", false)]
    public async Task CheckCultureDictionary(string language, bool expectedStatus = true, string? textToCheck = null)
    {
        var culture = new CultureInfo(language);
        Assert.NotNull(culture);

        if (expectedStatus)
        {
            await fixture.CheckLanguageDictionary(culture, textToCheck);
            Directory.CreateDirectory("Dictionaries");
            File.Move("ar_SR.dic", "Dictionaries/ar_SR.dic", true);
            File.Move("ar_SR.aff", "Dictionaries/ar_SR.aff", true);
            Assert.False(fixture.SpellCheckFactory.Contains(culture));
            fixture.SpellCheckFactory.DictionaryDirectory = "Dictionaries";
            await fixture.CheckLanguageDictionary(culture, textToCheck);
            
        }
        else
            Assert.False(fixture.SpellCheckFactory.Contains(culture));
    }

}