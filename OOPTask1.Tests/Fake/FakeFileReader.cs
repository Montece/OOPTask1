using System.IO;
using System.Text;

namespace OOPTask1.Tests.Fake;

internal sealed class FakeFileReader
{
    public StreamReader Reader { get; private set; }

    public FakeFileReader(string text)
    {
        var data = Encoding.UTF8.GetBytes(text);
        var stream = new MemoryStream(data);
        Reader = new StreamReader(stream);
    }
}