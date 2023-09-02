using System.Globalization;
using System.Reflection;
using System.Text;

namespace SpellCheck.Dictionaries;

public class SpellCheckFactory : ISpellCheckFactory
{
    public string DictionaryDirectory { get; set; } = string.Empty;
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
        var dictionary = GetStream(culture, "dic");
        if (dictionary is null)
            throw new Exception($"Dictionary for '{culture.Name}' not found.");
        
        var aff = GetStream(culture, "aff");
        if (aff is null)
            throw new Exception($"Affix for '{culture.Name}' not found.");
        
        return await SpellCheck.CreateFromStreams(dictionary, aff, ignoredWords);
    }

    private Stream? GetStream(CultureInfo culture, string extension)
    {
        var stream = GetFileStream(culture, extension);
        if (stream is not null)
            return stream;
        
        stream = GetResourceStream(culture, extension);
        return stream;
    }

    private Stream? GetFileStream(CultureInfo culture, string extension)
    {
        foreach (var fileName in GetFileNamesToCheck(culture, extension))
        {
            var path = Path.GetFullPath(Path.Combine(DictionaryDirectory, fileName));
            if (File.Exists(fileName))
                return File.OpenRead(fileName);
        }

        return null;
    }

    private Stream? GetResourceStream(CultureInfo culture, string extension)
    {
        foreach(var name in GetFileNamesToCheck(culture, extension))
        {
            var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream($"SpellCheck.Dictionaries.{name}");
            if (stream is not null)
                return stream;
        }

        return null;
    }

    public bool Contains(CultureInfo culture)
    {
        return GetStream(culture, "dic") is not null &&
            GetStream(culture, "aff") is not null;
    }

    public bool Contains(string language)
    {
        return Contains(new CultureInfo(language));
    }

    private string[] GetFileNamesToCheck(CultureInfo culture, string extension)
    {
        return new[]
        {
            $"{culture.Name}.{extension}",
            $"{culture.Name}.{extension}".Replace("-", "_"),
            $"{culture.Name}_{culture.Name}.{extension}",
            $"{culture.Name}-{culture.Name}.{extension}",
            $"{culture.EnglishName}.{extension}",
            $"{culture.NativeName}.{extension}",
            $"{culture.ThreeLetterISOLanguageName}.{extension}",
            $"{culture.ThreeLetterWindowsLanguageName}.{extension}",
            $"{culture.TwoLetterISOLanguageName}.{extension}",
        };
    }
}