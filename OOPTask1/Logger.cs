namespace OOPTask1
{
    /// <summary>
    /// Система логгирования
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Уровень логгирования по умолчанию
        /// </summary>
        private static LogLevel _defaultLogLevel;
        private static readonly List<ILogger> loggers = new();
        private static bool _initialized = false;

        /// <summary>
        /// Инициализация
        /// </summary>
        /// <param name="defaultLogLevel"> Уровень логгирования по умолчанию </param>
        public static void Initialize(LogLevel defaultLogLevel = LogLevel.Information)
        {
            _defaultLogLevel = defaultLogLevel;

            foreach (var logger in loggers)
                logger.Initialize();

            _initialized = true;
        }

        /// <summary>
        /// Зарегистрировать выходной логгер
        /// </summary>
        /// <param name="logger"> Логгер </param>
        /// <returns> Удалось ли зарегистрировать </returns>
        public static bool Register(ILogger logger)
        {
            if (_initialized)
                return false;

            if (loggers.Contains(logger))
                return false;

            loggers.Add(logger);

            return true;
        }

        /// <summary>
        /// Дерегистрировать выходной логгер
        /// </summary>
        /// <param name="logger"> Логгер </param>
        /// <returns> Удалось ли дерегистрировать </returns>
        public static bool Unregister(ILogger logger)
        {
            if (!loggers.Contains(logger))
                return false;

            loggers.Remove(logger);

            return true;
        }

        /// <summary>
        /// Запись лога с ошибкой
        /// </summary>
        /// <param name="ex"> Исключение </param>
        public static void Log(Exception ex) => Log(ex.Message, LogLevel.Error);

        /// <summary>
        /// Запись лога
        /// </summary>
        /// <param name="message"> Сообщение </param>
        public static void Log(object message) => Log(message, _defaultLogLevel);

        /// <summary>
        /// Запись лога
        /// </summary>
        /// <param name="message"> Сообщение </param>
        /// <param name="logLevel"> Уровень логгирования </param>
        public static void Log(object message, LogLevel logLevel)
        {
            if (!_initialized)
                return;

            var now = DateTime.Now;
            var fullMessage = $"[{now.ToShortDateString()} {now.ToShortTimeString()}] {message}";

            foreach (var logger in loggers)
                logger.Log(fullMessage, logLevel);
        }
    }
}
