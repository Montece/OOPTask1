namespace OOPTask1.Abstract
{
    /// <summary>
    /// Система подсчета количества слов в тексте
    /// </summary>
    internal interface IParsingManager
    {
        public bool Execute(FileInfo fileInfo);
    }
}