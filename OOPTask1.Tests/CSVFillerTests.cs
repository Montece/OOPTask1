using NLog.Filters;
using OOPTask1.Model;
using System.IO;
using Xunit;

namespace OOPTask1.Tests;

public sealed class CSVFillerTests
{
    [Fact]
    public void StartStopFilling_Success()
    {
        var filler = new CSVFiller();
        var fillingStart = false;
        var fillingStop = false;

        try
        {
            filler.StartFilling(new FileInfo("StringBuilder.txt"));
            fillingStart = filler.IsFilling;
        }
        finally
        {
            filler.StopFilling();
            fillingStop = !filler.IsFilling;
        }

        Assert.True(fillingStart == fillingStop);
    }

    [Fact]
    public void AddWord_Success()
    {
        var filler = new CSVFiller();
        var word = new Word("test");

        try
        {
            filler.StartFilling(new FileInfo("StringBuilder.txt"));
            filler.AddWord(word);
        }
        finally
        {
            filler.StopFilling();
        }

        Assert.True(filler.Records.Count(r => r.Word.CustomEquals(word)) != 0);
    }

    [Fact]
    public void TryGetWord_Success()
    {
        var filler = new CSVFiller();
        var word = new Word("test");

        try
        {
            filler.StartFilling(new FileInfo("StringBuilder.txt"));
            filler.AddWord(word);
        }
        finally
        {
            filler.StopFilling();
        }

        Assert.True(filler.TryGetRecord(word).Word == word);
    }
}