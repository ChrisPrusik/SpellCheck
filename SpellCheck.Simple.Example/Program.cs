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