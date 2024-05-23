namespace OOPTask1
{
    public class ParsingManager : IParsingManager
    {
        private readonly List<ParserBase> _parsers = new();

        public bool Register<T>() where T : ParserBase, new()
        {
            if (_parsers.Any(p => p is T))
                return false;

            var parser = new T();
            _parsers.Add(parser);

            return true;
        }

        public bool Unregister<T>() where T : ParserBase, new()
        {
            var parser = _parsers.FirstOrDefault(p => p is T);

            if (parser is null)
                return false;

            _parsers.Remove(parser);

            return true;
        }

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
