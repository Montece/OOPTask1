using NLog;
using OOPTask1.Abstract;

namespace OOPTask1;

public sealed class FileParsingManager : IParsingManager
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly List<IFileParser> _parsers = new();

    public bool Register(IFileParser parser)
    {
        ArgumentNullException.ThrowIfNull(parser);

        if (_parsers.Any(p => p.FileExtension.Equals(parser.FileExtension)))
        {
            return false;
        }

        _parsers.Add(parser);

        return true;
    }

    public bool Unregister(IFileParser parser)
    {
        ArgumentNullException.ThrowIfNull(parser);

        if (!_parsers.All(p => p.FileExtension.Equals(parser.FileExtension)))
        {
            return false;
        }

        _parsers.RemoveAll(p => p.FileExtension.Equals(parser.FileExtension));

        return true;
    }

    public bool Execute(FileInfo fileInfo)
    {
        ArgumentNullException.ThrowIfNull(fileInfo);
        
        try
        {
            if (string.IsNullOrEmpty(fileInfo.Extension))
            {
                throw new ArgumentException("File doesn't have extension", nameof(fileInfo));
            }

            var fileExtension = fileInfo.Extension[1..];
            var parsers = _parsers.Where(p => p.FileExtension.Equals(fileExtension)).ToList();

            if (parsers.Count == 0)
            {
                return false;
            }

            foreach (var parser in parsers)
            {
                parser.Parse(fileInfo);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex);
            return false;
        }
    }
}