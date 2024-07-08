using OOPTask1.Abstract;

namespace OOPTask1.Parsers;

public sealed class TxtParser(IStreamParser streamParser) : IFileParser
{
    public string FileExtension => "txt";

    public void Parse(FileInfo fileInfo)
    {
        using var fs = fileInfo.OpenRead();
        using var fr = new StreamReader(fs);

        using var csv = File.Create("output.csv");
        using var csvWriter = new StreamWriter(csv);
        streamParser.Parse(fr, csvWriter);
    }
}