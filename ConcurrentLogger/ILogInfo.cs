using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentLogger
{

    public interface ILogInfo
    {
        String Message { get; }
        String Time { get; }
        LogLevel LogLevel { get; }
    }

}