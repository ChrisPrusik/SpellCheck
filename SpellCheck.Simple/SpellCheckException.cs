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

using Markdig.Syntax;

namespace SpellCheck;

/// <summary>
/// The <see cref="SpellCheckException"/> class is thrown when an error occurs during spell-checking.
/// </summary>
public class SpellCheckException : Exception
{
    /// <summary>
    /// Gets the <see cref="MarkdownObject"/> that caused the exception.
    /// </summary>
    public readonly MarkdownObject MarkdownItem;
    
    /// <summary>
    /// Gets the column where the exception occurred.
    /// </summary>
    public readonly int Column;

    /// <summary>
    /// Gets the line where the exception occurred.
    /// </summary>
    public readonly int Line;

    /// <summary>
    /// Creates a new instance of the <see cref="SpellCheckException"/> class.
    /// </summary>
    /// <param name="markdownItem">the <see cref="MarkdownObject"/> that caused the exception.</param>
    /// <param name="message">Error message</param>
    /// <param name="inner">Inner exception (optional)</param>
    public SpellCheckException(MarkdownObject markdownItem, string message, Exception? inner = null) : 
        base($"Spell check error at {markdownItem.GetType().Name} started in line {markdownItem.Line}, column {markdownItem.Column}: {message}", inner)
    {
        MarkdownItem = markdownItem;
        Column = markdownItem.Column;
        Line = markdownItem.Line;
    }
}