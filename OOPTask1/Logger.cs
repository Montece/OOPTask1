namespace OOPTask1
{
    public static class Logger
    {
        private static LogLevel _defaultLogLevel;
        private static readonly List<ILogger> loggers = new();
        private static bool _initialized = false;

        public static void Initialize(LogLevel defaultLogLevel = LogLevel.Information)
        {
            _defaultLogLevel = defaultLogLevel;

            foreach (var logger in loggers)
                logger.Initialize();

            _initialized = true;
        }

        public static bool Register(ILogger logger)
        {
            if (_initialized)
                return false;

            if (loggers.Contains(logger))
                return false;

            loggers.Add(logger);

            return true;
        }

        public static bool Unregister(ILogger logger)
        {
            if (!loggers.Contains(logger))
                return false;

            loggers.Remove(logger);

            return true;
        }

        public static void Log(Exception ex) => Log(ex.Message, LogLevel.Error);

        public static void Log(object message) => Log(message, _defaultLogLevel);

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
