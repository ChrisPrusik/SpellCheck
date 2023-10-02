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

using System.Text.RegularExpressions;
using Markdig;
using Markdig.Syntax;

namespace SpellCheck.Extensions
{
    /// <summary>
    /// Provides extension methods for text manipulation and parsing.
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// Retrieves an array of words from the given text.
        /// </summary>
        public static string[] GetWords(this string text)
        {
            text = text.RemoveUrls().RemoveEmails();
            var matches = WordsRegex().Matches(text);
            var words = matches.Select(x => x.Value).ToList();
            return words.ToArray();
        }

        /// <summary>
        /// Retrieves an array of URLs from the given text.
        /// </summary>
        public static string[] GetUrls(this string text) =>
            UrlsRegex().Matches(text).Select(m => m.Value).ToArray();

        /// <summary>
        /// Removes URLs from the provided text.
        /// </summary>
        public static string RemoveUrls(this string checkText) =>
            UrlsRegex().Replace(checkText, "");

        /// <summary>
        /// Retrieves an array of emails from the given text.
        /// </summary>
        public static string[] GetEmails(this string text) =>
            EmailsRegex().Matches(text).Select(m => m.Value).ToArray();

        /// <summary>
        /// Removes emails from the provided text.
        /// </summary>
        public static string RemoveEmails(this string checkText) =>
            EmailsRegex().Replace(checkText, "");

        /// <summary>
        /// Retrieves a substring of the given text based on the provided Markdown item.
        /// </summary>
        public static string GetSubstring(this string text, MarkdownObject item) =>
            text.Substring(item.Span.Start, item.Span.Length);

        /// <summary>
        /// Parses the given text as a Markdown document.
        /// </summary>
        public static MarkdownDocument GetMarkdownDocument(this string text) =>
            Markdown.Parse(text, MarkdownPipeline);

        private static MarkdownPipeline MarkdownPipeline =>
            new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();

        /// <summary>
        /// Regular expression to match words.
        /// </summary>
        [GeneratedRegex(@"\b\p{L}+\b")]
        private static partial Regex WordsRegex();

        /// <summary>
        /// Regular expression to match URLs.
        /// </summary>
        [GeneratedRegex(@"(http|https|ftp|file)://[\w-]+(\.[\w-]+)+([\w.,@?^=%&amp;:/~+#-]*[\w@?^=%&amp;/~+#-])?")]
        private static partial Regex UrlsRegex();

        /// <summary>
        /// Regular expression to match emails.
        /// </summary>
        [GeneratedRegex(@"(\b[A-Za-z0-9#._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\b)")]
        private static partial Regex EmailsRegex();
    }
}
