using System.Text;
using NLog;
using OOPTask1.Model;

namespace OOPTask1
{
    /// <summary>
    /// Базовый класс конкретной реализации для анализа текста
    /// </summary>
    public abstract class TextFileParserBase
    {
        /// <summary>
        /// Расширение файла-источника текста (без точки)
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

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private bool _isFilling = false;
        private FileStream _csvStream = null;
        private ulong _allWordsCount = 0;
        private List<Record> _records = new(128);

        public void Execute(FileInfo fileInfo)
        {
            ArgumentNullException.ThrowIfNull(fileInfo);

            try
            {
                StartFilling(fileInfo);
                Parse(fileInfo);
            }
            finally
            {
                StopFilling();
            }  
        }

        /// <summary>
        /// Анализ файла
        /// </summary>
        protected abstract void Parse(FileInfo fileInfo);

        /// <summary>
        /// Начать вывод результатов анализа в файл
        /// </summary>
        private bool StartFilling(FileInfo fileInfo)
        {
            if (_isFilling)
            {
                return false;
            }

            ArgumentNullException.ThrowIfNull(fileInfo);
            
            var filenameWithoutExtension = Path.Combine(fileInfo.DirectoryName, Path.GetFileNameWithoutExtension(fileInfo.Name));

            _csvStream = new FileStream($"{filenameWithoutExtension}.{TARGET_FILE_EXTENSION}", FileMode.Create, FileAccess.Write);

            _isFilling = true;

            return true;
        }

        /// <summary>
        /// Остановить запись в файл
        /// </summary>
        private bool StopFilling()
        {
            if (!_isFilling)
            {
                return false;
            }

            if (_csvStream is null)
            {
                return false;
            }

            try
            {
                _csvStream.Close();
                _csvStream.Dispose();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return false;
            }

            _isFilling = false;

            return true;
        }

        /// <summary>
        /// Запомнить слово во внутренний буфер
        /// </summary>
        protected virtual void RecordWord(Word word)
        {
            var record = TryGetWord(word);

            if (record is null)
            {
                record = new Record(word, 1);
                _records.Add(record);
            }
            else
            {
                record.Count++;
            }

            _allWordsCount++;
        }

        /// <summary>
        /// Попытка получить запись о слове
        /// </summary>
        protected virtual Record? TryGetWord(Word word)
        {
            ArgumentNullException.ThrowIfNull(word);

            var record = _records.Where(r => r.Word != null && r.Word.Equals(word)).FirstOrDefault();

            return record;
        }

        /// <summary>
        /// Выгрузить все слова в файл
        /// </summary>
        protected virtual void FillAllWords(bool needSorting)
        {
            if (needSorting)
            {
                SortWords();
            } 

            foreach (var record in _records)
            {
                Fill(record);
            }
        }

        protected virtual void SortWords()
        {
            _records = _records.OrderByDescending(x => x.Count).ToList();
        }

        /// <summary>
        /// Сделать запись со словом и информацией в файл
        /// </summary>
        protected virtual bool Fill(Record record)
        {
            ArgumentNullException.ThrowIfNull(record);     

            if (!IsFilling  || _allWordsCount == 0)
            {
                return false;
            }

            var frequency = record.GetFrequency(_allWordsCount);
            var frequencyInPercents = frequency / 100;

            var frequencyText = frequency.ToString("0.0000").Replace(',', '.');
            var frequencyInPercentsText = $"{frequencyInPercents.ToString("0.000").Replace(',', '.')}%";
            var splitChar = ';';
            var line = $"{record.Word}{splitChar}{frequencyText}{splitChar}{frequencyInPercentsText}{Environment.NewLine}";

            var data = Encoding.UTF8.GetBytes(line);

            _csvStream.Write(data, 0, data.Length);
            _csvStream.Flush();

            _logger.Info($"Сделана запись '{line}'");

            return true;
        }

        ~TextFileParserBase()
        {
            if (!_isFilling)
            {
                return;
            }

            _csvStream?.Close();
            _csvStream?.Dispose();
        }
    }
}