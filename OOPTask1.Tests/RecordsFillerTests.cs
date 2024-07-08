using OOPTask1.Tests.Fake;
using Xunit;

namespace OOPTask1.Tests
{
    public sealed class RecordsFillerTests
    {
        [Fact]
        public void RecordsFiller_StartFilling()
        {
            var recordsFiller = new RecordsFiller();

            try
            {
                var fileWriter = new FakeFileWriter();
                recordsFiller.StartFilling(fileWriter.Writer);

                Assert.True(recordsFiller.IsFilling);
            }
            finally
            {
                recordsFiller.StopFilling();
            }
        }

        [Fact]
        public void RecordsFiller_StopFilling()
        {
            var recordsFiller = new RecordsFiller();

            try
            {
                var fileWriter = new FakeFileWriter();
                recordsFiller.StartFilling(fileWriter.Writer);
            }
            finally
            {
                recordsFiller.StopFilling();
                Assert.False(recordsFiller.IsFilling);
            }
        }

        [Fact]
        public void RecordsFiller_AddWord()
        {
            var recordsFiller = new RecordsFiller();
            var testWord = "Hello";

            try
            {
                var fileWriter = new FakeFileWriter();
                recordsFiller.StartFilling(fileWriter.Writer);
                recordsFiller.AddWord(new(testWord));

                Assert.Equal(testWord, recordsFiller.Records.ElementAt(0).Word.Value);
            }
            finally
            {
                recordsFiller.StopFilling();
            }
        }

        [Fact]
        public void RecordsFiller_TryGetRecord()
        {
            var recordsFiller = new RecordsFiller();
            var testWord = "Hello";

            try
            {
                var fileWriter = new FakeFileWriter();
                recordsFiller.StartFilling(fileWriter.Writer);
                recordsFiller.AddWord(new(testWord));
                var record = recordsFiller.TryGetRecord(new(testWord));
                Assert.Equal(testWord, record.Word.Value);
            }
            finally
            {
                recordsFiller.StopFilling();
            }
        }

        [Fact]
        public void RecordsFiller_FillAll()
        {
            var recordsFiller = new RecordsFiller();
            var testWord = "Hello";
            var expectedRecord = "Hello;1.0000;0.010%";

            try
            {
                var fileWriter = new FakeFileWriter();
                recordsFiller.StartFilling(fileWriter.Writer);
                recordsFiller.AddWord(new(testWord));
                recordsFiller.FillAll(false);
                var text = fileWriter.GetText();
                Assert.Equal(expectedRecord, text[0]);
            }
            finally
            {
                recordsFiller.StopFilling();
            }
        }
    }
}
