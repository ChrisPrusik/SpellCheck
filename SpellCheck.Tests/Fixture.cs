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

public class Fixture
{
    private const string dictionariesDirectory = "../../../../SpellCheck.Dictionaries";

    public readonly Dictionary<string, SpellCheck> SpellCheckers;

    public Fixture()
    {
        SpellCheckers = CreateSpellCheckers().Result;
    }

    private async Task<Dictionary<string, SpellCheck>> CreateSpellCheckers() =>
        new()
        {
            { "en", await CreateSpellCheck("en_GB", 
                "Arghul", "Arghul's", "babelfish", "i18n", "libelous", "Hyperlinking", "tm", "Smartypants") },
            { "pl", await CreateSpellCheck("pl_PL", "Arghul") }
        };

    private static async Task<SpellCheck> CreateSpellCheck(string language, params string[] ignoredWords)
    {
        var directory = Path.GetFullPath(dictionariesDirectory);
        Assert.True(Directory.Exists(directory), $"Directory '{directory}' does not exist.");

        var dictionaryPath = Path.Combine(directory, $"{language}.dic");
        Assert.True(File.Exists(dictionaryPath), $"File '{dictionaryPath}' does not exist.");

        var affixPath = Path.Combine(directory, $"{language}.aff");
        Assert.True(File.Exists(affixPath), $"File '{affixPath}' does not exist.");

        return await SpellCheck.CreateFromFiles(dictionaryPath, affixPath, ignoredWords);
    }
}