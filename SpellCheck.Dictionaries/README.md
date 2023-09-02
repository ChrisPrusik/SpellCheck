# SpellCheck.Directories.dll

The library contains a factory for creating SpellCheck objects 
based on the declared language or culture.

```csharp
var spellCheck = SpellCheckFactory.Create("en-US");
spellCheck.CheckTest("Hello world!");
```

or

```csharp
var spellCheck = SpellCheckFactory.Create(CultureInfo.CurrentCulture);
// For example: pl-PL
spellCheck.CheckTest("Żółw idzie drogą!");
```

## Supported languages

There are some built in dictionaries for the following languages:

| Language  | Culture | Dictionary | Affix file |
|-----------|---------|------------|------------|
| Deutsch   | de-DE   | de_DE.dic  | de_DE.aff  |
| English   | en-US   | en_US.dic  | en_US.aff  |
| Español   | es-ES   | es_ES.dic  | es_ES.aff  |
| Français  | fr-FR   | fr_FR.dic  | fr_FR.aff  |
| Italiano  | it-IT   | it_IT.dic  | it_IT.aff  |
| Polish    | pl-PL   | pl_PL.dic  | pl_PL.aff  |
| Português | pt-PT   | pt_PT.dic  | pt_PT.aff  |

If you need more languages, you can download them from [this GitHub repository](https://github.com/titoBouzout/Dictionaries)
and put them into destination directory to work with.

```csharp
var spellCheck = SpellCheckFactory.Create(@"C:\Dictionaries");
// there are two files in the C:\Dictionaries directory: ar.dic and ar.aff
spellCheck.CheckText("العالم العربي");
```

Many more languages are available on the Internet:
1. [OpenOffice.org](http://extensions.openoffice.org/en/dictionaries) (`*.osx` files)
2. [SoftMaker.com](https://www.softmaker.com/en/download/dictionaries) (`*.osx` files)

But you have to change downloaded files extension from `*.osx` to `*.zip` 
and unpack `*.dif` and `aff` files.






    