using System.Net.Security;
using System.Text;

namespace OOPTask1
{
    public abstract class ParserBase
    {
        public abstract string FileExtension { get; }

        public const string FILE_EXTENSION = ".csv";
        public bool IsFilling => _isFilling;
        
        private bool _isFilling = false;
        private FileStream _csvStream = null;

        protected abstract void Parse(string filename);

        protected bool StartFilling(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename));

            if (_isFilling)
                return false;

            var filenameWithoutExtension = Path.GetFileNameWithoutExtension(filename);

            _csvStream = new FileStream($"{filenameWithoutExtension}.{FILE_EXTENSION}", FileMode.Create, FileAccess.Write);

            _isFilling = true;

            return true;
        }

        protected bool Fill(string text, int frequency, int frequencyInPerc)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            if (!IsFilling)
                return false;

            var validCharacters = text.Any(c => !char.IsLetterOrDigit(c));

            if (!validCharacters)
                return false;

            var line = $"{text},{frequency},{frequencyInPerc}";
            var data = Encoding.UTF8.GetBytes(line);

            _csvStream.Write(data, 0, data.Length);
            _csvStream.Flush();

            return true;
        }

        protected bool StopFilling()
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