namespace OOPTask1
{
    /// <summary>
    /// Система подсчета количества слов в тексте
    /// </summary>
    public interface IParsingManager
    {
        /// <summary>
        /// Исполнение системы
        /// </summary>
        /// <param name="filePath"> Путь до файла с текстом </param>
        /// <returns> Удалось ли выполнить исполнение системы </returns>
        public bool Execute(string filePath);
    }
}