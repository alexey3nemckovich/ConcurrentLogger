using System.Threading;

namespace ConcurrentLogger
{

    public class LoggerTargetFlushingThreaPool : ILoggerTargetFlushingThreadPool
    {

        private Thread[] flushingThreads;
        private LogInfo[] logInfo;
        private ILoggerTarget[] loggerTargets;

        public LoggerTargetFlushingThreaPool(ILoggerTarget[] loggerTargets)
        {
            this.loggerTargets = loggerTargets;
            flushingThreads = new Thread[loggerTargets.Length];
        }

        public async void FlushAllTargets(LogInfo[] logsInfo)
        {
               
        }

        public void WaitAllTasksToFlush()
        {

        }

        public bool AllTargetsWereFlushed
        {
            get
            {
                foreach(Thread thread in flushingThreads)
                {
                    if (thread.IsAlive)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

    }

}