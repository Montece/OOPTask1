using NLog;
using OOPTask1.Model;

namespace OOPTask1
{
    /// <summary>
    /// Базовый класс конкретной реализации для анализа файла с текстом
    /// </summary>
    public abstract class FileParserBase
    {
        /// <summary>
        /// Расширение файла-источника текста (без точки)
        /// </summary>
        public abstract string FileExtension { get; }

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private CSVFiller _csvFiller = new();

        public void Execute(FileInfo fileInfo)
        {
            ArgumentNullException.ThrowIfNull(fileInfo);

            try
            {
                _csvFiller.StartFilling(fileInfo);
                Parse(fileInfo);
                _csvFiller.FillAll(needSorting: true);
            }
            finally
            {
                _csvFiller.StopFilling();
            }  
        }

        protected abstract void Parse(FileInfo fileInfo);
    
        protected void AddWord(Word word)
        {
            ArgumentNullException.ThrowIfNull(word);
            
            _csvFiller.AddWord(word);
        }
    }
}