namespace ConcurrentLogger
{

    public class Logger : ILogger
    {

        private ILoggerTargetFlushingThreadPool loggerTargetFlushingThreadPool;       
        private LogInfo[] logsInfo;
        private int bufferLimit;
        private int countEvents;

        public Logger(int bufferLimit, ILoggerTarget[] targets)
        {
            if (bufferLimit < 1)
            {
                throw new System.ArgumentException("Buffer limite should be a positive number");
            }
            loggerTargetFlushingThreadPool = new LoggerTargetFlushingThreaPool(targets);
            logsInfo = new LogInfo[bufferLimit];
            this.bufferLimit = bufferLimit;
        }

        public void Log(LogLevel logLevel, string message)
        {

            logsInfo[countEvents] = new LogInfo(logLevel, message);
            countEvents++;
            if(countEvents == bufferLimit)
            {
                if(!loggerTargetFlushingThreadPool.AllTargetsWereFlushed)
                {
                    loggerTargetFlushingThreadPool.WaitAllTasksToFlush();
                }
                loggerTargetFlushingThreadPool.FlushAllTargets(logsInfo);
            }
        }

        private void WriteLogsToTargets()
        {
            logsInfo = new LogInfo[bufferLimit];
            countEvents = 0;
        }

    }

}