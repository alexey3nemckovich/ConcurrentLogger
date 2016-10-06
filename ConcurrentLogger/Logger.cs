namespace ConcurrentLogger
{

    public class Logger : ILogger
    {

        private object threadLock;
        private ILoggerTargetFlushingTaskPool loggerTargetFlushingThreadPool;       
        private ILogInfo[] logsInfo;
        private int bufferLimit;
        private int countEvents;

        public Logger(int bufferLimit, ILoggerTarget[] targets)
        {
            if (bufferLimit < 1)
            {
                throw new System.ArgumentException("Buffer limite should be a positive number");
            }
            loggerTargetFlushingThreadPool = new LoggerTargetFlushingTaskPool(targets);
            logsInfo = new ILogInfo[bufferLimit];
            this.bufferLimit = bufferLimit;
            threadLock = new object();
            countEvents = 0;
        }

        public void Log(ILogInfo logInfo)
        {
            lock(threadLock)
            {
                logsInfo[countEvents] = logInfo;
                countEvents++;
                if (NeedToFlush())
                {
                    FlushAllTargets();
                }
            }
        }

        private bool NeedToFlush()
        {
            if(countEvents == bufferLimit)
            {
                return true;
            }
            return false;
        }

        private void FlushAllTargets()
        {
            WriteLogsToTargets();
            ResetCounter();
        }

        private void WriteLogsToTargets()
        {
            if (!loggerTargetFlushingThreadPool.AllTargetsWereFlushed)
            {
                loggerTargetFlushingThreadPool.WaitAllTasksToFlush();
            }
            loggerTargetFlushingThreadPool.FlushAllTargets(logsInfo);
        }

        private void ResetCounter()
        {
            countEvents = 0;
        }

    }

}