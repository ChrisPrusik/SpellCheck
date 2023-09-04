# SpellCheck Offline Library

Offline spell-checking words within texts, including Markdown tags. 
This library enables accurate detection of spelling errors 
while considering the presence of Markdown formatting.

The library utilizes Open Office's *.dic and *.aff files 
to support spell-checking in over 80 languages from around the world.

Additionally, you can refer to the SpellCheck.Dictionaries.dll, 
which is a library containing built-in languages for your convenience. 
This library enhances the spell-checking capability by providing preloaded 
dictionaries for various languages, streamlining the process of checking 
text accuracy in different linguistic contexts.

## Features

1. Accurate spell-checking for various languages.
2. Easy integration with Markdown documents.
3. Flexible options to customize dictionaries and ignored words.
4. Extensible and user-friendly API.

## How to use

The SpellCheck library can be used to perform various spell-checking tasks. 
Here's an example of how to get started:

```csharp
using SpellCheck;

// Create a SpellCheck instance
var spellChecker = SpellCheck.CreateFromFiles("en-US.dic", "en-US.aff");
spellChecker.SetIgnoredWords("bulba", "kotek");

// Perform spell-checking
var isCorrect = spellChecker.IsWordCorrect("hello"); // true
var isCorrect = spellChecker.IsWordCorrect("kotek"); // true
var isCorrect = spellChecker.IsWordCorrect("adsasd"); // false
var isTextCorrect = spellChecker.IsTextCorrect("This is an example text."); // true


// Get suggestions for an incorrect word
IEnumerable<string> suggestions = spellChecker.SuggestWord("incurrect");

// More advanced usage is possible as well
```