using System.Threading.Tasks;

namespace ConcurrentLogger
{

    public class LoggerTargetFlushingTaskPool : ILoggerTargetFlushingTaskPool
    {

        private Task<bool>[] flushingTasks;
        private ILoggerTarget[] loggerTargets;

        public LoggerTargetFlushingTaskPool(ILoggerTarget[] loggerTargets)
        {
            this.loggerTargets = loggerTargets;
            flushingTasks = new Task<bool>[loggerTargets.Length];
        }

        public void FlushAllTargets(ILogInfo[] logsInfo)
        {
            for(int i = 0; i < loggerTargets.Length; i++)
            {
                flushingTasks[i] = loggerTargets[i].FlushAsync(logsInfo);
            }
        }

        public void WaitAllTasksToFlush()
        {
            Task.WaitAll(flushingTasks);
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