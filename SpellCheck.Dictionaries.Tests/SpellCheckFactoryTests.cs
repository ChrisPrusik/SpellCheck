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
using Xunit;

namespace SpellCheck.Dictionaries.Tests;

public class SpellCheckFactoryTests
{
    private readonly string[] ignoredWords = { "babelfish" };
    private readonly ISpellCheckFactory spellCheckFactory = new SpellCheckFactory();
    
    [Theory]
    [InlineData("pl-PL")]
    [InlineData("en-US")]
    [InlineData("en-GB")]
    [InlineData("de-DE")]
    [InlineData("es-ES")]
    [InlineData("fr-FR")]
    [InlineData("it-IT")]
    [InlineData("pt-PT")]
    [InlineData("ar-SR")]
    [InlineData("ru_RU", false)]
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
    [InlineData("ar-SR")]
    [InlineData("ru_RU", false)]
    public void ContainsCultureDictionary(string language, bool expectedStatus = true)
    {
        var culture = new CultureInfo(language);
        Assert.NotNull(culture);
        
        if (expectedStatus)
            Assert.True(spellCheckFactory.Contains(culture));
        else
            Assert.False(spellCheckFactory.Contains(culture));
    }

    [Theory]
    [InlineData("ar-SR", true, "اكتشف السعودية بطرق متنوعة")]
    [InlineData("ar-SR", false, null, "Test")]
    [InlineData("en-GB", true, "Anyway, we're going to be open in three months.")]
    [InlineData("en-GB", true, null, "Test")]
    [InlineData("ru-RU", false)]
    [InlineData("ru-RU", true, "Остатки социализма в России", "Test")]
    public async Task CheckSpellCheckDictionary(string language, bool expectedStatus = true, 
        string? textToCheck = null, string? directory = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(directory) is false)
                spellCheckFactory.DictionaryDirectory = directory;

            if (expectedStatus)
                await CheckLanguageDictionary(language, textToCheck);
            else
                Assert.False(spellCheckFactory.Contains(language));
        }
        finally
        {
            spellCheckFactory.DictionaryDirectory = null;
        }
    }

    [Theory]
    [InlineData("ar-SR", true, "اكتشف السعودية بطرق متنوعة")]
    [InlineData("en-GB", true, "Anyway, we're going to be open in three months.")]
    [InlineData("ru-RU", false)]
    public async Task CheckCultureDictionary(string language, bool expectedStatus = true, string? textToCheck = null)
    {
        var culture = new CultureInfo(language);
        Assert.NotNull(culture);

        if (expectedStatus)
            await CheckLanguageDictionary(culture, textToCheck);
        else
            Assert.False(spellCheckFactory.Contains(culture));
    }
        
    private async Task CheckLanguageDictionary(string language, 
        string? textToCheck = null, bool expectedStatus = true)
    {
        Assert.True(spellCheckFactory.Contains(language));
        var spellCheck = await spellCheckFactory.CreateSpellCheck(language, ignoredWords);
        Assert.NotNull(spellCheck);
        Assert.True(spellCheck.DictionaryCount > 0);

        CheckText(spellCheck, textToCheck, expectedStatus);
    }
    
    private static void CheckText(ISpellCheck spellCheck, string? textToCheck, bool expectedStatus)
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

    private async Task CheckLanguageDictionary(CultureInfo culture, 
        string? textToCheck = null, bool expectedStatus = true)
    {
        Assert.True(spellCheckFactory.Contains(culture));
        var spellCheck = await spellCheckFactory.CreateSpellCheck(culture, ignoredWords);
        Assert.NotNull(spellCheck);
        Assert.True(spellCheck.DictionaryCount > 0);
        
        CheckText(spellCheck, textToCheck, expectedStatus);
    }
}