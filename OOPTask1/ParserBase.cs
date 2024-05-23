using System.Text;

namespace OOPTask1
{
    public abstract class ParserBase
    {
        public abstract string FileExtension { get; }

        private const string TARGET_FILE_EXTENSION = "csv";
        private const char SPLIT_CHAR = ';';
        public bool IsFilling => _isFilling;
        
        private bool _isFilling = false;
        private FileStream _csvStream = null;
        protected long wordsCount = 0;
        private Dictionary<string, long> _words = new(128);

        public abstract void Parse(string filename);

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

        protected virtual void RecordWord(string word)
        {
            (bool hasWord, long? count) = TryGetWordCount(word);

            if (hasWord && count.HasValue)
                _words[word] = count.Value + 1;
            else
                _words[word] = 1;

            wordsCount++;
        }

        protected virtual (bool hasWord, long? count) TryGetWordCount(string word)
        {
            if (string.IsNullOrEmpty(word))
                throw new ArgumentNullException(nameof(word));

            var hasWord = _words.TryGetValue(word, out long count);
            return (hasWord, hasWord ? count : null);
        }

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

        protected virtual void SortWords()
        {
            _words = _words.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

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