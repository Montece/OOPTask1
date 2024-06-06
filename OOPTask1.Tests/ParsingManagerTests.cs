using OOPTask1.Parsers;
using System.IO;
using Xunit;

namespace OOPTask1.Tests
{
    public class ParsingManagerTests
    {
        [Theory]
        [InlineData(["StringBuilder.txt", "StringBuilder.csv", "StringBuilder;0.0216;2.165%"])]
        public void ExecuteTXTParserTest(string inputFilename, string outputFilename, string assertValue)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, inputFilename);

            var parsingManager = new ParsingManager();
            parsingManager.Register<TXTParser>();
            parsingManager.Execute(filePath);

            var firstLine = File.ReadAllLines(outputFilename).FirstOrDefault();

            Assert.Equal(assertValue, firstLine);
        }
    }
}
