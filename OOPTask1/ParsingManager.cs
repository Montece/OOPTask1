﻿using NLog;
using OOPTask1.Abstract;

namespace OOPTask1;

/// <summary>
/// Система с различными анализаторами текста
/// </summary>
public sealed class ParsingManager : IParsingManager
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly List<FileParserBase> _parsers = new();

    public bool Register(FileParserBase parser)
    {
        ArgumentNullException.ThrowIfNull(parser);

        if (_parsers.Contains(parser))
        {
            return false;
        }

        _parsers.Add(parser);

        return true;
    }

    public bool Unregister(FileParserBase parser)
    {
        ArgumentNullException.ThrowIfNull(parser);

        if (!_parsers.Contains(parser))
        {
            return false;
        }

        _parsers.Remove(parser);

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

            var fileExtension = fileInfo.Extension.Substring(1);
            var parsers = _parsers.Where(p => p.FileExtension.Equals(fileExtension)).ToList();

            if (parsers.Count == 0)
            {
                return false;
            }

            foreach (var parser in parsers)
            {
                parser.Execute(fileInfo);
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