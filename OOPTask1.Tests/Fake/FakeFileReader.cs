using System.IO;
using System.Text;

namespace OOPTask1.Tests.Fake;

internal sealed class FakeFileReader
{
    public StreamReader Reader { get; private set; }

    private readonly string _text;
    private readonly MemoryStream _stream;

    public FakeFileReader(string text)
    {
        _text = text;

        var data = Encoding.UTF8.GetBytes(_text);
        _stream = new MemoryStream(data);
        Reader = new StreamReader(_stream);
    }
}