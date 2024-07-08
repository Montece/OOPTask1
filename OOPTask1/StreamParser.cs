﻿using NLog;
using OOPTask1.Abstract;
using OOPTask1.Model;
using System.Text;

namespace OOPTask1;

public sealed class StreamParser : IStreamParser
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly RecordsFiller _csvFiller = new();

    public void Parse(StreamReader sourceStream, StreamWriter targetStream)
    {
        ArgumentNullException.ThrowIfNull(sourceStream);
        ArgumentNullException.ThrowIfNull(targetStream);

        try
        {
            _csvFiller.StartFilling(targetStream);
            Analyse(sourceStream);
            _csvFiller.FillAll(needSorting: true);
        }
        finally
        {
            _csvFiller.StopFilling();
        }
    }

    private void Analyse(StreamReader sourceStream)
    {
        var sb = new StringBuilder();

        while (!sourceStream.EndOfStream)
        {
            var letterIndex = sourceStream.Read();

            if (letterIndex == -1)
            {
                break;
            }

            var letter = (char)letterIndex;

            if (char.IsControl(letter))
            {
                continue;
            }

            if (!char.IsLetterOrDigit(letter))
            {
                var wordStr = sb.ToString();

                if (!string.IsNullOrEmpty(wordStr))
                {
                    var word = new Word(wordStr);
                    AddWord(word);
                }

                sb.Clear();
            }
            else
            {
                sb.Append(letter);
            }
        }
    }

    private void AddWord(Word word)
    {
        ArgumentNullException.ThrowIfNull(word);

        _csvFiller.AddWord(word);
    }
}
