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

using System.Diagnostics.CodeAnalysis;
using Markdig.Syntax;
using Xunit;
using SpellCheck.Extensions;

namespace SpellCheck.Simple.Tests;

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
        Assert.Equal(paragraphBlockCount, markdownDocument.OfType<ParagraphBlock>().ToList().Count);
    }

    [Theory]
    [InlineData(typeof(HeadingBlock), "text markdown")]
    [InlineData(typeof(HeadingBlock), "# title", "# title")]
    [InlineData(typeof(HeadingBlock), markDownText, "# Header", "## Another header")]
    [SuppressMessage("Usage", "xUnit1010:The value is not convertible to the method parameter type")]
    public void GetSubstring(Type type, string text, params string[] expectedResult)
    {
        var markdownDocument = text.GetMarkdownDocument();
        var results = markdownDocument.Descendants().Where(x => x.GetType() == type).ToList();
        if (expectedResult.Length != results.Count)
            throw new Exception($"Expected {expectedResult.Length} results, but got {results.Count}.");
        
        for(var index = 0; index < results.Count; index++)
            Assert.Equal(expectedResult[index], text.GetSubstring(results[index]));
    }
}