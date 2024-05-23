namespace OOPTask1
{
    public interface ILogger
    {
        public void Initialize();

        public void Log(object message, LogLevel logLevel);
    }
}
