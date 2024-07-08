namespace OOPTask1.Abstract;

public interface IFileParser
{
    public string FileExtension { get; }

    public void Parse(FileInfo fileInfo);
}
