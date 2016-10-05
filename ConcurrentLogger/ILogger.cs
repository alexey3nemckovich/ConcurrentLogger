namespace ConcurrentLogger
{

    interface ILogger
    {
        void Log(LogLevel logLevel, string message);
    }

}