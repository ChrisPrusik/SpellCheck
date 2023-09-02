# SpellCheck

SpellCheck nuget package

## Links

1. [Hunspell website](https://hunspell.github.io)
2. [Hunspell .NET Core GitHub](https://github.com/aarondandy/WeCantSpell.Hunspell/)
3. [Markdig markdown parser](https://github.com/xoofx/markdig)
4. [Dictionaries to download](https://www.softmaker.com/en/download/dictionaries)

## More dictionaries

https://www.wearecogworks.com/blog/nuget-with-github-packages-tutorial/

# Language dictionaries

SpellCheck works with dictionaries and affix files comming from Open Office [Hunspell format](https://hunspell.github.io/).

## How to download more language dictionaries

Go to the website [softmaker.com/en/download/dictionaries](https://www.softmaker.com/en/download/dictionaries) and download dictionary you wish. There are dictionaries cover the following languages: English (United States), English (United Kingdom), French (France), French (Canada), German, Swiss German, Italian, Spanish (Spain), Spanish (Latin America), Portuguese (Portugal), Portuguese (Brazil), Dutch, Danish, Swedish, Norwegian, Russian, Greek and Arabic, and many more. 

A total of 85 language dictionaries are available.

After downloading, change the file name extension `.sox` to `.zip` and open the file. You should see some files inside. 
Unpack `*.dic` and `*.aff` files to the destination directory, an that's all. You can use these files by `SpellCheck` and `SpellCheckFactory` classes.