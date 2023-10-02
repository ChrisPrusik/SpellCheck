# SpellCheck Offline Library

Offline spell-checking of words within texts, including Markdown tags. 
This library enables accurate detection of spelling errors while considering the presence of Markdown formatting.

The library utilizes OpenOffice's *.dic and *.aff files to support spell-checking in over 80 languages from around the world.

The library comes with built-in support for languages such as 
de-DE, en-GB, en-US, es-ES, fr-FR, it-IT, pl-PL, and pt-PT for your convenience. 
It enhances the spell-checking capability by providing preloaded dictionaries, 
thus streamlining the process of checking text for accuracy across different languages.

> NOTE: Feel free to make a pull request. 

## SpellCheck.Simple nuget package

The `SpellCheck.Simple` library can be used to perform various spell-checking tasks. 
Here's an example of how to get started:

Install the [SpellCheck.Simple](https://www.nuget.org/packages/SpellCheck.Simple) nuget package
from the package manager console:

```cmd
dotnet add package SpellCheck.Simple
```

Just create an instance of the `SpellCheck` class and use it.

```csharp
using SpellCheck;

// Create a SpellCheck instance
var spellChecker = await SpellChecker.CreateFromFiles("en_US.dic", "en_US.aff");
spellChecker.SetIgnoredWords("bulba", "kotek");

// Perform spell-checking on a word
Console.WriteLine(spellChecker.IsWordCorrect("hello")); // true
Console.WriteLine(spellChecker.IsWordCorrect("kotek")); // true
Console.WriteLine(spellChecker.IsWordCorrect("adsasd")); // false

// Perform spell-checking on a text
Console.WriteLine(spellChecker.IsTextCorrect("This is an example text.")); // true
Console.WriteLine(spellChecker.IsTextCorrect("This is an ixample text.")); // false

// Get suggestions for an incorrect word
var suggestions = spellChecker.SuggestWord("worl"); 
Console.WriteLine(string.Join(", ", suggestions)); // work, world

// More advanced usage is possible as well
```

Use this class in the way above if you want to use your own dictionaries 
and don't want to use built-in dictionaries.

## SpellCheck.Dictionaries nuget package

A much simpler way to use the SpellCheck class is to use the built-in dictionaries.

Install the [SpellCheck.Dictionaries](https://www.nuget.org/packages/SpellCheck) nuget package
from the package manager console:

```cmd
dotnet add package SpellCheck.Dictionaries
```

Just create an instance of the `SpellChecker` class and use it.

```csharp
using SpellCheck.Dictionaries;

var factory = new SpellCheckFactory();
// Create a SpellCheck instance
var arSpellChecker = await factory.CreateSpellChecker("ar_SR");
var enSpellChecker = await factory.CreateSpellChecker("en_US");

Console.WriteLine(enSpellChecker.IsTextCorrect("hello world")); // true
Console.WriteLine(arSpellChecker.IsTextCorrect("متنوعة")); // true
```

There are some built in dictionaries for the following languages:

| Language     | Culture | Dictionary | Affix file |
|--------------|---------|------------|------------|
| Deutsch      | de-DE   | de_DE.dic  | de_DE.aff  |
| English (US) | en-US   | en_US.dic  | en_US.aff  |
| English (GB) | en-GB   | en_GB.dic  | en_GB.aff  |
| Español      | es-ES   | es_ES.dic  | es_ES.aff  |
| Français     | fr-FR   | fr_FR.dic  | fr_FR.aff  |
| Italiano     | it-IT   | it_IT.dic  | it_IT.aff  |
| Polish       | pl-PL   | pl_PL.dic  | pl_PL.aff  |
| Português    | pt-PT   | pt_PT.dic  | pt_PT.aff  |

## Custom dictionaries

```csharp
using SpellCheck.Dictionaries;

var factory = new SpellCheckFactory("C:\Dictionaries");

// Create a SpellCheck instance from files ar-SR.dic and ar-SR.aff
// in the C:\Dictionaries directory
var spellChecker = factory.CreateSpellCheck("ar-SR");
```

If the directory is not specified, the current directory is used.

`SpellCheck` works with dictionaries and affix files coming from Open Office [Hunspell format](https://hunspell.github.io/).
If you need more languages, you can download them from [this GitHub repository](https://github.com/titoBouzout/Dictionaries)
and put them into destination directory to work with. These files are in UTF-8 format.

Another way is to go to the website [softmaker.com](https://www.softmaker.com/en/download/dictionaries) 
and download dictionary you wish. A total of 85 language dictionaries are available. 
After downloading, change the file name extension `.sox` to `.zip` and open the file. You should see some files inside.
Unpack `*.dic` and `*.aff` files to the destination directory, an that's all. 
You can use these files by `SpellCheck` and `SpellCheckFactory` classes.

If you're using the `SpellCheck` class alone, 
the `*.dic` and `*.aff` files should be in UTF-8 format. 
If they're not, you can convert them using the  
[iconv tool](https://www.fileformat.info/tip/linux/iconv.htm) or 
include the following code before the first use of the `SpellCheck` class:

```csharp
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
```

This step is unnecessary if you're using `SpellCheckFactory` class.

## Class dependency diagram

![Class diagram](class-diagram.png)

## Links

1. [Hunspell website](https://hunspell.github.io)
2. [Hunspell .NET GitHub](https://github.com/aarondandy/WeCantSpell.Hunspell/)
3. [Markdig markdown parser GitHub](https://github.com/xoofx/markdig)
4. [Dictionaries to download](https://www.softmaker.com/en/download/dictionaries)