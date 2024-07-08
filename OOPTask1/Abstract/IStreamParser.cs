namespace OOPTask1.Abstract;

public interface IStreamParser
{
    void Parse(StreamReader sourceStream, StreamWriter targetStream);
}
