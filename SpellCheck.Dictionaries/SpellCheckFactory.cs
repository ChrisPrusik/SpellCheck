using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SpellCheck.Dictionaries.Extensions;

namespace SpellCheck.Dictionaries;

public class SpellCheckFactory : ISpellCheckFactory
{
    public string? DictionaryDirectory { get; set; } = null;
    
    public SpellCheckFactory(string? dictionaryDirectory = null)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        if (string.IsNullOrWhiteSpace(dictionaryDirectory) is true) 
            return;
        
        if (Directory.Exists(dictionaryDirectory))
            DictionaryDirectory = dictionaryDirectory;
        else
            throw new DirectoryNotFoundException($"Directory '{dictionaryDirectory}' does not exist.");
    }

    public async Task<SpellCheck> CreateSpellCheck(string language, 
        params string[] ignoredWords)
    {
        var culture = new CultureInfo(language);
        if (culture is null)
            throw new Exception($"Unknown language '{language}'.");

        return await CreateSpellCheck(culture, ignoredWords);
    }

    public async Task<SpellCheck> CreateSpellCheck(CultureInfo culture, 
        params string[] ignoredWords)
    {
        var dictionary = culture.GetSpellCheckStream(DictionaryTypes.Dictionary, DictionaryDirectory);
        if (dictionary is null)
            throw new Exception($"Dictionary for '{culture.Name}' not found.");
        
        var aff = culture.GetSpellCheckStream(DictionaryTypes.Affix, DictionaryDirectory);
        if (aff is null)
            throw new Exception($"Affix for '{culture.Name}' not found.");
        
        return await SpellCheck.CreateFromStreams(dictionary, aff, ignoredWords);
    }

    public bool Contains(CultureInfo culture) =>
        culture.GetSpellCheckStream(DictionaryTypes.Dictionary, DictionaryDirectory) is not null &&
        culture.GetSpellCheckStream(DictionaryTypes.Affix, DictionaryDirectory) is not null;

    public bool Contains(string language) => 
        Contains(new CultureInfo(language));
}