using System.IO;
using OOPTask1.Parsers;
using Xunit;

namespace OOPTask1.Tests;

public sealed class FileParsingManagerTests
{
    [Fact]
    public void FileParsingManager_Register()
    {
        var parsingManager = new FileParsingManager();
        var streamParser = new StreamParser();
        var txtParser = new TxtParser(streamParser);
        parsingManager.Register(txtParser);

        var result = parsingManager.Execute(new FileInfo("StringBuilder.txt"));
        
        Assert.True(result);
    }

    [Fact]
    public void FileParsingManager_Unregister()
    {
        var parsingManager = new FileParsingManager();
        var streamParser = new StreamParser();
        var txtParser = new TxtParser(streamParser);
        parsingManager.Register(txtParser);
        parsingManager.Unregister(txtParser);

        var result = parsingManager.Execute(new FileInfo("StringBuilder.txt"));

        Assert.False(result);
    }

    [Theory]
    [InlineData(["StringBuilder.txt", "output.csv", "в,0.0308,0.000%"])]
    public void ExecuteTXTParserTest(string inputFilename, string outputFilename, string expectedValue)
    {
        var fileInfo = new FileInfo(Path.Combine(Environment.CurrentDirectory, inputFilename));

        var parsingManager = new FileParsingManager();
        var streamParser = new StreamParser();
        var txtParser = new TxtParser(streamParser);
        parsingManager.Register(txtParser);
        parsingManager.Execute(fileInfo);

        var firstLine = File.ReadAllLines(outputFilename).FirstOrDefault();

        Assert.Equal(expectedValue, firstLine);
    }
}