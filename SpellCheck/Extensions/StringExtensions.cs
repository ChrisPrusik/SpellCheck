using System.Text.RegularExpressions;
using Markdig;
using Markdig.Syntax;

namespace SpellCheck.Extensions;

public static class StringExtensions
{
    private const string wordRegex = @"\b\p{L}+\b";
    private const string emailRegex = @"(\b[A-Za-z0-9#._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\b)";
    private const string urlRegex = 
        @"(http|https|ftp|file)://[\w-]+(\.[\w-]+)+([\w.,@?^=%&amp;:/~+#-]*[\w@?^=%&amp;/~+#-])?";

    public static string[] GetWords(this string text)
    {
        text = text.RemoveUrls().RemoveEmails();
        var matches = Regex.Matches(text, wordRegex);
        var words = matches.Select(x => x.Value).ToList();
        return words.ToArray();
    }

    public static string[] GetUrls(this string text) => 
        Regex.Matches(text, urlRegex).Select(m => m.Value).ToArray();

    public static string RemoveUrls(this string checkText) => 
        Regex.Replace(checkText, urlRegex, "");
    
    public static string[] GetEmails(this string text) => 
        Regex.Matches(text, emailRegex).Select(m => m.Value).ToArray();

    public static string RemoveEmails(this string checkText) => 
        Regex.Replace(checkText, emailRegex, "");
    
    public static string GetSubstring(this string text, MarkdownObject item) => 
        text.Substring(item.Span.Start, item.Span.Length);
    
    public static MarkdownDocument GetMarkdownDocument(this string text) => 
        Markdown.Parse(text, MarkdownPipeline);
    
    private static MarkdownPipeline MarkdownPipeline => 
        new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();
}