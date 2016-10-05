using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentLogger
{

    interface ILoggerTargetFlushingThreadPool
    {
        void FlushAllTargets(LogInfo[] logsInfo);
        bool AllTargetsWereFlushed { get; }
        void WaitAllTasksToFlush();
    }

}