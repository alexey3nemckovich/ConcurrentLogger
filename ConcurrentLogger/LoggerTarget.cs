using System;
using System.IO;
using System.Threading.Tasks;

namespace ConcurrentLogger
{

    public class LoggerTarget : ILoggerTarget
    {

        public static int count = 1;
        public int id = count++;
        private String fileName;

        public LoggerTarget()
        {
            this.fileName = @".\Target" + id + "LogFile.txt";
        }

        public LoggerTarget(String fileName)
        {
            this.fileName = fileName;
        }

        public bool Flush(ILogInfo[] logsInfo)
        {
            foreach (LogInfo logInfo in logsInfo)
            {
                System.Console.WriteLine("[{0}] {1} {2}. id - {3}", logInfo.Time, logInfo.LogLevel, logInfo.Message, id);
            }
            /*StreamWriter streamWriter = new StreamWriter(fileName, true);
            foreach(ILogInfo logInfo in logsInfo)
            {
                streamWriter.WriteLine("[{0}] {1} {2}.", logInfo.Time, logInfo.LogLevel, logInfo.Message);
            }
            streamWriter.Flush();
            streamWriter.Close();*/
            return true;
        }

        private bool Flush(Object logsInfoObj)
        {
            ILogInfo[] logsInfo = (ILogInfo[])logsInfoObj;
            return Flush(logsInfo);
        }

        public Task<bool> FlushAsync(ILogInfo[] logsInfo)
        {
            Task<bool> flushTask = new Task<bool>(Flush, logsInfo);
            flushTask.Start();
            return flushTask;
        }

    }

}