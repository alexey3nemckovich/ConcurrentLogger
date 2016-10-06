using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentLogger
{

    public class FlushingThreadData : IFlushingThreadData<int>
    {

        private ILogInfo[] logsInfo;
        private int threadNumber;

        public FlushingThreadData(ILogInfo[] logsInfo, int data)
        {
            this.logsInfo = logsInfo;
            this.threadNumber = data;
        }

        public int Data
        {
            get
            {
                return threadNumber;
            }
        }

        public ILogInfo[] LogsInfo
        {
            get
            {
                return logsInfo;
            }
        }

    }

}