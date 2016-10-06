using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ConcurrentLogger
{

    public class LoggerTargetFlushingTaskPool : ILoggerTargetFlushingTaskPool<int>
    {

        private Task<bool>[] flushingTasks;
        private ILoggerTarget[] loggerTargets;
        private object threadLocker;
        private SortedSet<int> threadNumbersSortedSet;

        public LoggerTargetFlushingTaskPool(ILoggerTarget[] loggerTargets)
        {
            this.loggerTargets = loggerTargets;
            flushingTasks = new Task<bool>[loggerTargets.Length];
            threadLocker = new object();
            threadNumbersSortedSet = new SortedSet<int>();
        }

        public void FlushAllTargets(IFlushingThreadData<int> flushingThreadData)
        {
            lock(threadLocker)
            {
                threadNumbersSortedSet.Add(flushingThreadData.Data);
                while (threadNumbersSortedSet.Min != flushingThreadData.Data)
                {
                    Monitor.Wait(threadLocker);
                }
                WaitAllTasksToFlush();
                for (int i = 0; i < loggerTargets.Length; i++)
                {
                    //loggerTargets[i].Flush(flushingThreadData.LogsInfo);
                    flushingTasks[i] = loggerTargets[i].FlushAsync(flushingThreadData.LogsInfo);
                }
                threadNumbersSortedSet.Remove(flushingThreadData.Data);
                Monitor.Pulse(threadLocker);
            }
        }

        public void WaitAllTasksToFlush()
        {
            if(!AllTargetsWereFlushed)
            {
                Task.WaitAll(flushingTasks);
            }
        }

        public bool AllTargetsWereFlushed
        {
            get
            {
                foreach(Task<bool> task in flushingTasks)
                {
                    if (task != null && !task.IsCompleted)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

    }

}