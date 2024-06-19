using OOPTask1.Model;
using Xunit;

namespace OOPTask1.Tests
{
    public class RecordTests
    {
        [Fact]
        public void Create_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new Model.Record(null, 0));
        }

        [Theory]
        [InlineData(["Привет"])]
        [InlineData(["Hi"])]
        public void Create_GoodValue(string wordStr)
        {
            var word = new Word(wordStr);
            var model = new Model.Record(word, 1);

            Assert.Equal(model.Word.Value, wordStr);
        }

        [Theory]
        [InlineData(10, 0.1d)]
        public void GetFrequency_GoodValue(ulong allWordsCount, ulong assertFrequency)
        {
            var word = new Word("test");
            var model = new Model.Record(word, 1);
            var frequency = model.GetFrequency(allWordsCount);

            Assert.Equal(assertFrequency, frequency);
        }
    }
}
