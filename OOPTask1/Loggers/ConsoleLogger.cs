namespace OOPTask1.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public void Initialize()
        {
            
        }

        public void Log(object message, LogLevel logLevel)
        {
            WriteLine(message, GetColorForLogLevel(logLevel));
        }

        protected virtual ConsoleColor GetColorForLogLevel(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Debug => ConsoleColor.Gray,
                LogLevel.Information => ConsoleColor.White,
                LogLevel.Warning => ConsoleColor.Yellow,
                LogLevel.Error => ConsoleColor.Red,
                LogLevel.Critical => ConsoleColor.Red,
                _ => ConsoleColor.White,
            };
        }

        protected virtual void WriteLine(object message, ConsoleColor messageColor)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = messageColor;
            Console.WriteLine(message);
            Console.ForegroundColor = oldColor;
        }
    }
}
