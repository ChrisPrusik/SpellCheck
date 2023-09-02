using Markdig;
using Markdig.Syntax;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Xunit;
using SpellCheck.Extensions;

namespace SpellCheck.Tests;

public class StringExtensionsTests
{
    private const string markDownText = """
    # Header
                                        
    First paragraph with email alibaba#123@gmail.com
                                        
    Second paragraph with url http://alibaba.com/ototo
                                        
    ## Another header
    """;
    
    [Theory]
    [InlineData("Hello world", "Hello", "world")]
    [InlineData("https://alibaba.com żółwik", "żółwik")]
    [InlineData("https://alibaba.com ")]
    [InlineData("https://alibaba.com beta@gmail.com")]
    public void GetWords(string text, params string[] expectedWords) => 
        Assert.Equal(expectedWords, text.GetWords());

    [Theory]
    [InlineData("https://alibaba.com żółwik", " żółwik")]
    [InlineData("https://alibaba.com/ żółwik", " żółwik")]
    [InlineData("https://alibaba.com/ototo żółwik", " żółwik")]
    [InlineData("acha https://alibaba.com/ototo żółwik", "acha  żółwik")]
    public void RemoveUrls(string text, string expectedResult) => 
        Assert.Equal(expectedResult, text.RemoveUrls());

    [Theory]
    [InlineData("alibaba@gmail.com żółwik", " żółwik")]
    [InlineData("alibaba#123@gmail.com żółwik", " żółwik")]
    [InlineData("alibaba.weteran@gmail.com żółwik", " żółwik")]
    [InlineData("acha alibaba@gmail.com żółwik", "acha  żółwik")]
    public void RemoveEmails(string text, string expectedResult) => 
        Assert.Equal(expectedResult, text.RemoveEmails());
    
    
    [Theory]
    [InlineData("https://alibaba.com żółwik", "https://alibaba.com")]
    [InlineData("https://alibaba.com/ żółwik", "https://alibaba.com/")]
    [InlineData("https://alibaba.com/ototo żółwik", "https://alibaba.com/ototo")]
    [InlineData("acha https://alibaba.com/ototo żółwik", "https://alibaba.com/ototo")]
    [InlineData("acha żółwik")]
    public void GetUrls(string text, params string[] expectedResult) => 
        Assert.Equal(expectedResult, text.GetUrls());

    [Theory]
    [InlineData("alibaba@gmail.com żółwik", "alibaba@gmail.com")]
    [InlineData("alibaba#123@gmail.com żółwik", "alibaba#123@gmail.com")]
    [InlineData("alibaba.weteran@gmail.com żółwik", "alibaba.weteran@gmail.com")]
    [InlineData("acha alibaba@gmail.com żółwik", "alibaba@gmail.com")]
    [InlineData("acha żółwik")]
    public void GetEmails(string text, params string[] expectedResult) => 
        Assert.Equal(expectedResult, text.GetEmails());
    
    [Theory]
    [InlineData(markDownText, 2, 2)]
    [InlineData("Hello world", 0, 1)]
    [InlineData("# Hello world", 1, 0)]
    public void GetMarkdownDocument(string text, int headingBlockCount, int paragraphBlockCount)
    {
        var markdownDocument = text.GetMarkdownDocument();
        Assert.NotNull(markdownDocument);
        Assert.Equal(headingBlockCount, markdownDocument.OfType<HeadingBlock>().ToList().Count);
        Assert.Equal(paragraphBlockCount, markdownDocument.OfType<ParagraphBlock>().ToList().Count());
    }

    [Theory]
    [InlineData(typeof(HeadingBlock), "text markdown")]
    [InlineData(typeof(HeadingBlock), "# title", "# title")]
    [InlineData(typeof(HeadingBlock), markDownText, "# Header", "## Another header")]
    public void GetSubstring(Type type, string text, params string[] expectedResult)
    {
        var markdownDocument = text.GetMarkdownDocument();
        var results = markdownDocument.Descendants().Where(x => x.GetType() == type).ToList();
        if (expectedResult.Length != results.Count)
            throw new Exception($"Expected {expectedResult.Length} results, but got {results.Count}.");
        
        for(int index = 0; index < results.Count; index++)
            Assert.Equal(expectedResult[index], text.GetSubstring(results[index]));
    }
}