using System;
using System.Threading.Tasks;

namespace ConcurrentLogger
{

    public interface ILoggerTarget
    {
        bool Flush(ILogInfo[] logsInfo);
        Task<bool> FlushAsync(ILogInfo[] logs);
    }

}