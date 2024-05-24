namespace OOPTask1
{
    /// <summary>
    /// Система с различными анализаторами текста
    /// </summary>
    public class ParsingManager : IParsingManager
    {
        private readonly List<ParserBase> _parsers = new();

        /// <summary>
        /// Регистрация анализатора по типу
        /// </summary>
        /// <returns> Удалось ли зарегистрировать </returns>
        public bool Register<T>() where T : ParserBase, new()
        {
            if (_parsers.Any(p => p is T))
                return false;

            var parser = new T();
            _parsers.Add(parser);

            return true;
        }

        /// <summary>
        /// Дерегистрация анализатора по типу
        /// </summary>
        /// <returns> Удалось ли дерегистрировать </returns>
        public bool Unregister<T>() where T : ParserBase, new()
        {
            var parser = _parsers.FirstOrDefault(p => p is T);

            if (parser is null)
                return false;

            _parsers.Remove(parser);

            return true;
        }

        /// <summary>
        /// Выполнить анализ
        /// </summary>
        /// <param name="filePath"> Путь до файла</param>
        /// <returns></returns>
        public virtual bool Execute(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Logger.Log($"Файл по пути '{filePath}' не существует!", LogLevel.Warning);
                    return false;
                }  

                var fileExtension = Path.GetExtension(filePath);
                var parser = _parsers.FirstOrDefault(p => p.FileExtension.Equals(fileExtension));

                if (parser is null)
                    return false;

                parser.Parse(filePath);

                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString(), LogLevel.Error);
                return false;
            }
        }
    }
}
