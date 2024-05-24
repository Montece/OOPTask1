namespace OOPTask1
{
    /// <summary>
    /// Интерфейс для системы логгирования
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Инициализация
        /// </summary>
        public void Initialize();

        /// <summary>
        /// Запись лога
        /// </summary>
        /// <param name="message"> Сообщение для записи </param>
        /// <param name="logLevel"> Уровень лога </param>
        public void Log(object message, LogLevel logLevel);
    }
}
