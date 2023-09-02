using System.Globalization;
using System.Threading.Tasks;

namespace SpellCheck.Dictionaries;

public interface ISpellCheckFactory
{
    string DictionaryDirectory { get; set; }
    Task<SpellCheck> CreateSpellCheck(CultureInfo culture, params string[] ignoredWords);
    Task<SpellCheck> CreateSpellCheck(string language, params string[] ignoredWords);
    bool Contains(CultureInfo culture);
    bool Contains(string language);
}