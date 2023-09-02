using System;
using System.Globalization;
using System.Threading.Tasks;
using Xunit;

namespace SpellCheck.Dictionaries.Tests;

public class Fixture
{
    private readonly string[] ignoredWords = new[] {
        "babelfish"
    };

    public SpellCheckFactory SpellCheckFactory = new();

    public Fixture()
    {

    }
        
    public async Task CheckLanguageDictionary(string language, 
        string? textToCheck = null, bool expectedStatus = true)
    {
        Assert.True(SpellCheckFactory.Contains(language));
        var spellCheck = await SpellCheckFactory.CreateSpellCheck(language, ignoredWords);
        Assert.NotNull(spellCheck);
        Assert.True(spellCheck.DictionaryCount > 0);

        CheckText(spellCheck, textToCheck, expectedStatus);
    }
    
    private void CheckText(SpellCheck spellCheck, string? textToCheck, bool expectedStatus)
    {
        if (string.IsNullOrWhiteSpace(textToCheck))
            return;
        
        if (expectedStatus)
            spellCheck.CheckText(textToCheck);
        else
        {
            try
            {
                spellCheck.CheckText(textToCheck);
                throw new Exception($"Expected SpellCheckException for '{textToCheck}'.");
            }
            catch(SpellCheckException)
            {
            }
        }
    }

    public async Task CheckLanguageDictionary(CultureInfo culture, 
        string? textToCheck = null, bool expectedStatus = true)
    {
        Assert.True(SpellCheckFactory.Contains(culture));
        var spellCheck = await SpellCheckFactory.CreateSpellCheck(culture, ignoredWords);
        Assert.NotNull(spellCheck);
        Assert.True(spellCheck.DictionaryCount > 0);
        
        CheckText(spellCheck, textToCheck, expectedStatus);
    }

}