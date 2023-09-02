using System;
using Markdig;
using Markdig.Syntax;

namespace SpellCheck;

public class SpellCheckException : Exception
{
    public readonly MarkdownObject MarkdownItem;
    public readonly int Column;
    public readonly int Line;

    public SpellCheckException(MarkdownObject markdownItem, string message) : 
        base($"Spell check error at {markdownItem.GetType().Name} started in line {markdownItem.Line}, column {markdownItem.Column}: {message}")
    {
        MarkdownItem = markdownItem;
        Column = markdownItem.Column;
        Line = markdownItem.Line;
    }
}