using NLog;
using OOPTask1.Model;
using System.Text;

namespace OOPTask1
{
    public sealed class CSVFiller
    {
        public bool IsFilling => _isFilling;
        public IReadOnlyCollection<Record> Records => _records;

        private const string TARGET_FILE_EXTENSION = "csv";
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private bool _isFilling = false;
        private FileStream _csvStream = null;
        private ulong _allWordsCount = 0;
        private List<Record> _records = new(128);

        public bool StartFilling(FileInfo fileInfo)
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

        public void AddWord(Word word)
        {
            ArgumentNullException.ThrowIfNull(word);

            var record = TryGetRecord(word);

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

        public Record? TryGetRecord(Word word)
        {
            ArgumentNullException.ThrowIfNull(word);

            var record = _records.Where(r => r.Word != null && r.Word.Equals(word)).FirstOrDefault();

            return record;
        }

        public void FillAll(bool needSorting)
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

        private bool Fill(Record record)
        {
            ArgumentNullException.ThrowIfNull(record);

            if (!IsFilling || _allWordsCount == 0)
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

        private void SortWords()
        {
            _records = _records.OrderByDescending(x => x.Count).ToList();
        }

        public bool StopFilling()
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

        ~CSVFiller()
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
