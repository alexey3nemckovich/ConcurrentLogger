using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentLogger
{

    interface ILoggerTargetFlushingTaskPool<T>
    {
        void FlushAllTargets(IFlushingThreadData<T> flushingThreadData);
        bool AllTargetsWereFlushed { get; }
        void WaitAllTasksToFlush();
    }

}