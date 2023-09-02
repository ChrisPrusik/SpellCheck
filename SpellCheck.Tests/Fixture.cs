using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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

    private async Task<Dictionary<string, SpellCheck>> CreateSpellCheckers()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        return new Dictionary<string, SpellCheck>()
        {
            { "en", await CreateSpellCheck("en_GB", 
                "Arghul", "Arghul's", "babelfish", "i18n", "libelous", "Hyperlinking", "tm", "Smartypants") },
            { "pl", await CreateSpellCheck("pl_PL", "Arghul") }
        };
    }

    private async Task<SpellCheck> CreateSpellCheck(string language, params string[] ignoredWords)
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