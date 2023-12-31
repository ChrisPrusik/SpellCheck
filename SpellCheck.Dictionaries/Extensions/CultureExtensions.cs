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
using System.Reflection;

namespace SpellCheck.Dictionaries.Extensions;

/// <summary>
///   Culture extensions.
/// </summary>
public static class CultureExtensions
{
    /// <summary>
    ///   Checks if the specified dictionary exists.
    /// </summary>
    public static bool SpellCheckExists(this CultureInfo culture,
        DictionaryTypes type = DictionaryTypes.Dictionary, string? directory = null) =>
        culture.ResourceSpellCheckExists(type) || culture.FileSpellCheckExists(type, directory);

    /// <summary>
    ///   Checks if the specified dictionary file exists.
    /// </summary>
    public static bool FileSpellCheckExists(this CultureInfo culture,
        DictionaryTypes type = DictionaryTypes.Dictionary, string? directory = null)
    {
        var filePath = culture.GetFilePath(type, directory);
        return File.Exists(filePath);
    }

    /// <summary>
    ///   Checks if the specified dictionary resource exists.
    /// </summary>
    public static bool ResourceSpellCheckExists(this CultureInfo culture,
        DictionaryTypes type = DictionaryTypes.Dictionary)
    {
        var assembly = GetSpellCheckResourcesAssembly();
        var fullName = culture.GetSpellCheckResourceName(type, assembly);
        return assembly?.GetManifestResourceInfo(fullName) is not null;
    }

    /// <summary>
    ///   Gets the dictionary stream.
    /// </summary>
    public static Stream? GetSpellCheckStream(this CultureInfo culture, 
        DictionaryTypes type = DictionaryTypes.Dictionary, string? directory = null)
    {
        var stream = culture.GetSpellCheckFileStream(type, directory);
        if (stream is not null)
            return stream;
        
        stream = culture.GetSpellCheckResourceStream(type);
        return stream;
    }

    /// <summary>
    ///  Gets the dictionary file stream.
    /// </summary>
    public static Stream? GetSpellCheckFileStream(this CultureInfo culture, 
        DictionaryTypes type = DictionaryTypes.Dictionary, string? directory = null)
    {
        var filePath = culture.GetFilePath(type, directory);
        return File.Exists(filePath) ? File.OpenRead(filePath) : null;
    }
    
    private static string? GetFilePath(this CultureInfo culture, 
        DictionaryTypes type = DictionaryTypes.Dictionary, string? directory = null) =>
        GetFilePath(culture.GetDictionaryFileName(type), directory);

    private static string? GetFilePath(string fileName, string? directory)
    {
        var filePath = Path.GetFullPath(fileName);
        if (string.IsNullOrWhiteSpace(directory) is false && Directory.Exists(directory))
            filePath = Path.GetFullPath(Path.Combine(directory, fileName));

        return filePath;
    }

    /// <summary>
    ///  Gets the dictionary resource stream.
    /// </summary>
    public static Stream? GetSpellCheckResourceStream(this CultureInfo culture, 
        DictionaryTypes type = DictionaryTypes.Dictionary)
    {
        var assembly = GetSpellCheckResourcesAssembly();
        var fullName = culture.GetSpellCheckResourceName(type, assembly);
        return assembly?.GetManifestResourceStream(fullName);
    }

    private static Assembly? GetSpellCheckResourcesAssembly() => 
        Assembly.GetAssembly(typeof(SpellCheckFactory));

    private static string GetSpellCheckResourceName(this CultureInfo culture, DictionaryTypes type, Assembly? assembly)
    {
        var prefix = assembly?.FullName?.Split(',')[0];
        var name = culture.GetDictionaryFileName(type).Replace("-", "_");
        var fullName = $"{prefix}.{name}";
        return fullName;
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