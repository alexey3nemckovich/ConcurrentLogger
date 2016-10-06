using System;

namespace ConcurrentLogger
{

    public struct LogInfo : ILogInfo
    {
        public LogLevel logLevel;
        public string message;
        public DateTime eventTime;

        public LogInfo(LogLevel logLevel, string message)
        {
            this.logLevel = logLevel;
            this.message = message;
            this.eventTime = DateTime.Now;
        }

        public LogLevel LogLevel
        {
            get
            {
                return logLevel;
            }
        }

        public String Message
        {
            get
            {
                return message;
            }
        }

        public DateTime Time
        {
            get
            {
                return eventTime;
            }
        }

    }

}