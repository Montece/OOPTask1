using System.IO;

namespace OOPTask1.Tests.Fake;

internal sealed class FakeFileWriter
{
    public StreamWriter Writer { get; private set; }

    private readonly MemoryStream _stream;

    public FakeFileWriter()
    {
        _stream = new MemoryStream();
        Writer = new StreamWriter(_stream);
    }

    public string[] GetText()
    {
        List<string> lines = new();

        using var reader = new StreamReader(_stream);
        _stream.Position = 0;

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (!string.IsNullOrEmpty(line))
            {
                lines.Add(line);
            }
        }

        return lines.ToArray();
    }
}