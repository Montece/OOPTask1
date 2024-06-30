using OOPTask1.Parsers;
using System.IO;
using Xunit;

namespace OOPTask1.Tests;

public sealed class ParsingManagerTests
{
    [Fact]
    public void Register_Success()
    {
        var parsingManager = new ParsingManager();
        var txtParser = new TXTParser();
        parsingManager.Register(txtParser);

        var result = parsingManager.Execute(new FileInfo("StringBuilder.txt"));

        Assert.True(result);
    }

    [Fact]
    public void Unregister_Success()
    {
        var parsingManager = new ParsingManager();
        var txtParser = new TXTParser();
        parsingManager.Register(txtParser);
        parsingManager.Unregister(txtParser);

        var result = parsingManager.Execute(new FileInfo("StringBuilder.txt"));

        Assert.False(result);
    }

    [Theory]
    [InlineData(["StringBuilder.txt", "StringBuilder.csv", "StringBuilder;0.0216;2.165%"])]
    public void ExecuteTXTParserTest(string inputFilename, string outputFilename, string assertValue)
    {
        var fileInfo = new FileInfo(Path.Combine(Environment.CurrentDirectory, inputFilename));

        var parsingManager = new ParsingManager();
        parsingManager.Register(new TXTParser());
        parsingManager.Execute(fileInfo);

        var firstLine = File.ReadAllLines(outputFilename).FirstOrDefault();
        var startsWith = firstLine.StartsWith("в;");

        Assert.True(startsWith);
    }
}