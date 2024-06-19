using NLog.Filters;
using OOPTask1.Model;
using System.IO;
using Xunit;

namespace OOPTask1.Tests
{
    public class CSVFillerTests
    {
        [Fact]
        public void StartStopFilling_Success()
        {
            var filler = new CSVFiller();
            bool fillingStart = false;
            bool fillingStop = false;

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

            Assert.True(fillingStart == fillingStop != false);
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

            Assert.True(filler.Records.Where(r => r.Word.Equals(word)).Count() != 0);
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
}
