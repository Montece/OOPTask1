using OOPTask1.Tests.Fake;
using Xunit;

namespace OOPTask1.Tests;

public sealed class StreamParserTests
{
    [Fact]
    public void StreamParser_Parse()
    {
        var streamParser = new StreamParser();

        var reader = new FakeFileReader("123 456 ");
        var writer = new FakeFileWriter();

        streamParser.Parse(reader.Reader, writer.Writer);

        var lines = writer.GetText();
        Assert.Equal("456,0.5000,0.005%", lines[1]);
    }
}