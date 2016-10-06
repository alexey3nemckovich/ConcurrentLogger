using System;

namespace ConcurrentLogger
{

    public interface ILogInfo
    {
        String Message { get; }
        DateTime Time { get; }
        LogLevel LogLevel { get; }
    }

}