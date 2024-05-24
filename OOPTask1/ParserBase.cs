using System.Text;

namespace OOPTask1
{
    /// <summary>
    /// Базовый класс конкретной реализации для анализа текста
    /// </summary>
    public abstract class ParserBase
    {
        /// <summary>
        /// Разрешение файла-источника текста
        /// </summary>
        public abstract string FileExtension { get; }

        /// <summary>
        /// Расширение файла для результатов анализа
        /// </summary>
        private const string TARGET_FILE_EXTENSION = "csv";
        /// <summary>
        /// Символ-разделитель параметров в строке результатов анализа
        /// </summary>
        private const char SPLIT_CHAR = ';';
        /// <summary>
        /// Выводится ли сейчас информация в файл
        /// </summary>
        public bool IsFilling => _isFilling;
        
        private bool _isFilling = false;
        private FileStream _csvStream = null;
        protected long wordsCount = 0;
        private Dictionary<string, long> _words = new(128);

        /// <summary>
        /// Анализ файла
        /// </summary>
        /// <param name="filename"> Имя файла </param>
        public abstract void Parse(string filename);

        /// <summary>
        /// Начать вывод результатов анализа в файл
        /// </summary>
        /// <param name="filename"> Имя файла </param>
        /// <returns> Удалось ли начать вывод </returns>
        /// <exception cref="ArgumentNullException" />
        protected virtual bool StartFilling(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename));

            if (_isFilling)
                return false;

            var filenameWithoutExtension = Path.GetFileNameWithoutExtension(filename);

            _csvStream = new FileStream($"{filenameWithoutExtension}.{TARGET_FILE_EXTENSION}", FileMode.Create, FileAccess.Write);

            _isFilling = true;

            return true;
        }

        /// <summary>
        /// Запомнить слово во внутренний буфер
        /// </summary>
        /// <param name="word"> Слово </param>
        protected virtual void RecordWord(string word)
        {
            (bool hasWord, long? count) = TryGetWordCount(word);

            if (hasWord && count.HasValue)
                _words[word] = count.Value + 1;
            else
                _words[word] = 1;

            wordsCount++;
        }

        /// <summary>
        /// Попытка получить количество повторений для слова
        /// </summary>
        /// <param name="word"> Слово </param>
        /// <returns> (Было ли уже такое слово, Количество слов) </returns>
        /// <exception cref="ArgumentNullException" />
        protected virtual (bool hasWord, long? count) TryGetWordCount(string word)
        {
            if (string.IsNullOrEmpty(word))
                throw new ArgumentNullException(nameof(word));

            var hasWord = _words.TryGetValue(word, out long count);
            return (hasWord, hasWord ? count : null);
        }

        /// <summary>
        /// Выгрузить все слова в файл
        /// </summary>
        /// <param name="sorted"> Нужна ли сортировка слов </param>
        protected virtual void FillAllWords(bool sorted)
        {
            if (sorted)
                SortWords();

            foreach (var pair in _words)
            {
                var frequency = (double)pair.Value / wordsCount;
                Fill(pair.Key, frequency, frequency * 100);
            }
        }

        /// <summary>
        /// Сортировка
        /// </summary>
        protected virtual void SortWords()
        {
            _words = _words.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Сделать запись со словом и информацией в файл
        /// </summary>
        /// <param name="text"> Слово </param>
        /// <param name="frequency"> Частота появления слова </param>
        /// <param name="frequencyInPerc"> Частота появления слова (в %) </param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        protected virtual bool Fill(string text, double frequency, double frequencyInPerc)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            if (!IsFilling)
                return false;

            var invalidCharacters = text.Any(char.IsLetterOrDigit);

            if (!invalidCharacters)
                return false;

            var frequencyText = frequency.ToString("0.0000").Replace(',', '.');
            var frequencyInPercText = frequencyInPerc.ToString("0.000").Replace(',', '.');
            var line = $"{text}{SPLIT_CHAR}{frequencyText}{SPLIT_CHAR}{frequencyInPercText}%";
            var data = Encoding.UTF8.GetBytes(line + Environment.NewLine);

            _csvStream.Write(data, 0, data.Length);
            _csvStream.Flush();

            Logger.Log($"Сделана запись '{line}'");

            return true;
        }

        /// <summary>
        /// Остановить запись в файл
        /// </summary>
        /// <returns></returns>
        protected virtual bool StopFilling()
        {
            if (!_isFilling)
                return false;

            if (_csvStream is null)
                return false;

            _csvStream.Close();
            _csvStream.Dispose();

            _isFilling = false;

            return true;
        }

        ~ParserBase()
        {
            if (!_isFilling)
                return;

            _csvStream?.Close();
            _csvStream?.Dispose();
        }
    }
}