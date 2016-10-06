using System;
using System.Collections.Generic;
using System.IO;
using ConcurrentLogger;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestConcurrentLogger
{

    [TestClass]
    public class ConcurrentLoggerTests
    {

        static Logger logger;

        static void TestThreadFunction()
        {
            for (int i = 0; i < 4; i++)
            {
                logger.Log(new LogInfo(LogLevel.INFO, "task " + i + " of thread №" + Thread.CurrentThread.ManagedThreadId + " Start"));
                Thread.Sleep(500);
                logger.Log(new LogInfo(LogLevel.INFO, "task " + i + " of thread №" + Thread.CurrentThread.ManagedThreadId + " End"));
            }
        }
        
        [TestMethod]
        public void TestLoggingChronology()
        {
            int countThreads = 3;
            String logFileName = @".\Log";
            logger = new Logger(2, new LoggerTarget[] { new LoggerTarget(logFileName) });
            ThreadManager.ExecuteAndWaitAllThreads(countThreads, TestThreadFunction);
            bool allThreadsLoggedInCorrectChronology = AllThreadsLoggedInCorrectChronology(countThreads, logFileName);
            Assert.AreEqual(allThreadsLoggedInCorrectChronology, true);
        }

        private bool AllThreadsLoggedInCorrectChronology(int countThreads, String logFileName)
        {
            Dictionary<int, DateTime> threadsLastLoggingTime = new Dictionary<int, DateTime>();
            StreamReader streamReader = new StreamReader(String.Format(@".\{0}1.txt", logFileName));
            DateTime lastLogTime = new DateTime();
            while (!streamReader.EndOfStream)
            {
                String line = streamReader.ReadLine();
                DateTime time = Convert.ToDateTime(line.Substring(1, line.LastIndexOf(']') - 1));
                if(lastLogTime == null)
                {
                    lastLogTime = time;
                }
                else
                {
                    if(lastLogTime > time)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }

}