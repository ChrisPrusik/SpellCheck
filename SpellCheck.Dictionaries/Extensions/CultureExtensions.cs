using System.Globalization;
using System.Reflection;

namespace SpellCheck.Dictionaries.Extensions;

public static class CultureExtensions
{

    public static Stream? GetSpellCheckStream(this CultureInfo culture, 
        DictionaryTypes type = DictionaryTypes.Dictionary, string? directory = null)
    {
        var stream = culture.GetSpellCheckFileStream(type, directory);
        if (stream is not null)
            return stream;
        
        stream = culture.GetSpellCheckResourceStream(type);
        return stream;
    }

    public static Stream? GetSpellCheckFileStream(this CultureInfo culture, 
        DictionaryTypes type = DictionaryTypes.Dictionary, string? directory = null)
    {
        var fileName = culture.GetDictionaryFileName(type);
        var filePath = GetFilePath(fileName, directory);
        if (File.Exists(filePath))
            return File.OpenRead(filePath);

        return null;
    }

    private static string? GetFilePath(string fileName, string? directory)
    {
        var filePath = Path.GetFullPath(fileName);
        if (string.IsNullOrWhiteSpace(directory) is false && Directory.Exists(directory))
            filePath = Path.GetFullPath(Path.Combine(directory, fileName));

        if (File.Exists(filePath))
            return filePath;

        return null;
    }

    public static Stream? GetSpellCheckResourceStream(this CultureInfo culture, 
        DictionaryTypes type = DictionaryTypes.Dictionary)
    {
        var assembly = Assembly.GetAssembly(typeof(SpellCheckFactory));
        var prefix = assembly?.FullName?.Split(',')[0];
        var name = culture.GetDictionaryFileName(type);
        var stream = assembly?.GetManifestResourceStream($"{prefix}.{name}");
        if (stream is not null)
            return stream;

        return null;
    }
    
    private static string GetDictionaryFileName(this CultureInfo culture, DictionaryTypes type) =>
        type switch
        {
            DictionaryTypes.Dictionary => culture.GetDictionaryFileName("dic"),
            DictionaryTypes.Affix => culture.GetDictionaryFileName("aff"),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    
    private static string GetDictionaryFileName(this CultureInfo culture, string extension) =>
        $"{culture.Name}.{extension}".Replace("-", "_");

}