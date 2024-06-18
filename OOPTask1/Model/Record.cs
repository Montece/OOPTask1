namespace OOPTask1.Model
{
    /// <summary>
    /// Запись с информацией о слове и частоте его появления
    /// </summary>
    public class Record
    {
        public Word Word { get; }
        public ulong Count { get; set; }

        public Record(Word word, ulong count)
        {
            ArgumentNullException.ThrowIfNull(word);

            Word = word;
            Count = count;
        }

        public double GetFrequency(ulong allWordsCount)
        {
            return (double)Count / allWordsCount;
        }
    }
}
