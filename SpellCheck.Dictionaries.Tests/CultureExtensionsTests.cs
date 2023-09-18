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
    [InlineData("ar-SR", DictionaryTypes.Dictionary)]
    [InlineData("ar-SR", DictionaryTypes.Affix)]
    [InlineData("pl-PL", DictionaryTypes.Dictionary, false)]
    [InlineData("pl-PL", DictionaryTypes.Affix, false)]
    [InlineData("ru-RU", DictionaryTypes.Dictionary, true, "Test")]
    [InlineData("ru-RU", DictionaryTypes.Affix, true, "Test")]
    [InlineData("en-GB", DictionaryTypes.Dictionary, false, "Test")]
    [InlineData("en-GB", DictionaryTypes.Affix, false, "Test")]
    public void FileSpellCheckExists(string language, DictionaryTypes type, bool expectedStatus = true, string? directory = null)
    {
        var culture = new CultureInfo(language);
        Assert.NotNull(culture);
        var exists = culture.FileSpellCheckExists(type, directory);
        if (expectedStatus)
            Assert.True(exists);
        else
            Assert.False(exists);
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
    [InlineData("ar-SR", DictionaryTypes.Dictionary, false)]
    [InlineData("ar-SR", DictionaryTypes.Affix, false)]
    public void ResourceSpellCheckExists(string language, DictionaryTypes type, bool expectedStatus = true)
    {
        var culture = new CultureInfo(language);
        Assert.NotNull(culture);
        
        var exists = culture.ResourceSpellCheckExists(type);
        if (expectedStatus)
            Assert.True(exists);
        else
            Assert.False(exists);
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
    
    [Theory]
    [InlineData("pl-PL", DictionaryTypes.Dictionary)]
    [InlineData("pl-PL", DictionaryTypes.Affix)]
    [InlineData("ar-SR", DictionaryTypes.Dictionary)]
    [InlineData("ar-SR", DictionaryTypes.Affix)]
    [InlineData("ru-RU", DictionaryTypes.Dictionary, false)]
    [InlineData("ru-RU", DictionaryTypes.Affix, false)]
    public void SpellCheckExists(string language, DictionaryTypes type, bool expectedStatus = true)
    {
        var culture = new CultureInfo(language);
        Assert.NotNull(culture);

        var exists = culture.SpellCheckExists(type);
        if (expectedStatus)
            Assert.True(exists);
        else
            Assert.False(exists);
    }
}