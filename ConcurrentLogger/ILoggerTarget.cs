using System.Threading.Tasks;

namespace ConcurrentLogger
{

    public interface ILoggerTarget
    {
        bool Flush(LogInfo[] logs);
        Task<bool> FlyshAsync(LogInfo[] logs);
    }

}