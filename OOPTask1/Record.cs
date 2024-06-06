namespace OOPTask1
{
    /// <summary>
    /// Запись с информацией о слове и частоте его появления
    /// </summary>
    public class Record(Word? word, int count, double frequency)
    {
        public Word? Word { get; set; } = word;
        public int Count { get; set; } = count;
        public double Frequency { get; set; } = frequency;
        public double FrequencyInPercents => Frequency * 100;

        /// <summary>
        /// Символ-разделитель параметров в строке результатов анализа
        /// </summary>
        private const char SPLIT_CHAR = ';';

        public override string ToString()
        {
            var frequencyText = Frequency.ToString("0.0000").Replace(',', '.');
            var frequencyInPercText = FrequencyInPercents.ToString("0.000").Replace(',', '.');
            var line = $"{Word?.Value}{SPLIT_CHAR}{frequencyText}{SPLIT_CHAR}{frequencyInPercText}%";
            return line;
        }
    }
}
