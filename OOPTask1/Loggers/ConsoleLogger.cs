namespace OOPTask1.Loggers
{
    /// <summary>
    /// Система логгирования в консольное окно
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        /// <inheritdoc />
        public void Initialize()
        {
            
        }

        /// <inheritdoc />
        public void Log(object message, LogLevel logLevel)
        {
            WriteLine(message, GetColorForLogLevel(logLevel));
        }

        /// <summary>
        /// Получить цвет текста консольного окна на основе уровня лога
        /// </summary>
        /// <param name="logLevel"> Уровень лога </param>
        /// <returns> Цвет текста </returns>
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

        /// <summary>
        /// Вывод цветного сообщения в консоль
        /// </summary>
        /// <param name="message"> Текст сообщения </param>
        /// <param name="messageColor"> Цвет текста </param>
        protected virtual void WriteLine(object message, ConsoleColor messageColor)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = messageColor;
            Console.WriteLine(message);
            Console.ForegroundColor = oldColor;
        }
    }
}
