using System;

namespace ConcurrentLogger
{

    public struct LogInfo
    {
        public LogLevel logLevel;
        public string message;
        public string eventTime;

        public LogInfo(LogLevel logLevel, string message)
        {
            this.logLevel = logLevel;
            this.message = message;
            this.eventTime = DateTime.Now.ToString("h:mm:ss tt");
        }

    }

}