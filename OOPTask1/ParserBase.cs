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
        /// Выводится ли сейчас информация в файл
        /// </summary>
        public bool IsFilling => _isFilling;
        
        private bool _isFilling = false;
        private FileStream _csvStream = null;
        protected long wordsCount = 0;
        private List<Record> _records = new(128);

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

            var filenameWithoutExtension = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename));

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
            var record = TryGetWord(word);

            if (record is null)
            {
                record = new Record(new(word), 1, 0);
                _records.Add(record);
            }
            else
            {
                record.Count++;
            }

            wordsCount++;
        }

        /// <summary>
        /// Попытка получить запись о слове
        /// </summary>
        /// <param name="word"> Слово </param>
        /// <returns> Запись о слове </returns>
        /// <exception cref="ArgumentNullException" />
        protected virtual Record? TryGetWord(string word)
        {
            if (string.IsNullOrEmpty(word))
                throw new ArgumentNullException(nameof(word));

            var record = _records.Where(w => w.Word != null && w.Word.Value.Equals(word)).FirstOrDefault();
            return record;
        }

        /// <summary>
        /// Выгрузить все слова в файл
        /// </summary>
        /// <param name="sorted"> Нужна ли сортировка слов </param>
        protected virtual void FillAllWords(bool sorted)
        {
            if (sorted)
                SortWords();

            foreach (var record in _records)
            {
                var frequency = (double)record.Count / wordsCount;
                record.Frequency = frequency;
                Fill(record);
            }
        }

        /// <summary>
        /// Сортировка
        /// </summary>
        protected virtual void SortWords()
        {
            _records = _records.OrderByDescending(x => x.Frequency).ToList();
        }

        /// <summary>
        /// Сделать запись со словом и информацией в файл
        /// </summary>
        /// <param name="text"> Слово </param>
        /// <param name="frequency"> Частота появления слова </param>
        /// <param name="frequencyInPerc"> Частота появления слова (в %) </param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        protected virtual bool Fill(Record record)
        {
            if (record is null || record.Word is null || string.IsNullOrEmpty(record.Word.Value))
                throw new ArgumentNullException(nameof(record));

            if (!IsFilling)
                return false;

            if (!record.Word.IsValid())
                return false;

            var line = record.ToString() + Environment.NewLine;
            var data = Encoding.UTF8.GetBytes(line);

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