using System;
using System.Threading;

namespace ConcurrentLogger
{

    public class Logger : ILogger
    {
        
        private object threadLock;
        private ILoggerTargetFlushingTaskPool<int> loggerTargetFlushingThreadPool;
        private ILogInfo[] logsInfo;
        private int bufferLimit;
        private int countEvents;
        private int lastStartedThreadNumber;

        public Logger(int bufferLimit, LoggerTarget[] targets)
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
            lastStartedThreadNumber = 0;
        }

        public void Log(ILogInfo logInfo)
        {
            lock (threadLock)
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
            if (countEvents == bufferLimit)
            {
                return true;
            }
            return false;
        }

        private void FlushAllTargets()
        {
            IFlushingThreadData<int> threadData = new FlushingThreadData(logsInfo, lastStartedThreadNumber++);
            ThreadPool.QueueUserWorkItem(WriteLogsToTargets, threadData);
            ResetCounter();
        }

        private void WriteLogsToTargets(Object flushingThreadDataObj)
        {
            IFlushingThreadData<int> flushingThreadData = (IFlushingThreadData<int>)flushingThreadDataObj;
            loggerTargetFlushingThreadPool.FlushAllTargets(flushingThreadData);
        }

        private void ResetCounter()
        {
            logsInfo = new ILogInfo[bufferLimit];
            countEvents = 0;
        }

    }

}