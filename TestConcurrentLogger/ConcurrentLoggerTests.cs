using System;
using System.IO;
using System.Net;
using ConcurrentLogger;
using System.Threading;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestConcurrentLogger
{

    [TestClass]
    public class ConcurrentLoggerTests
    {

        static Logger logger;
        static IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        static int portNumber = 50000;

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
        public void TestFileLoggerChronology()
        {
            int countThreads = 3;
            String logFileName = @".\Log";
            logger = new Logger(2, new ILoggerTarget[] { new LoggerTarget(logFileName) });
            ThreadManager.StartAndWaitAllThreads(new Thread[countThreads], TestThreadFunction);
            bool allThreadsLoggedInCorrectChronology = CheckFileLogChronology(logFileName);
            Assert.AreEqual(allThreadsLoggedInCorrectChronology, true);
        }

        [TestMethod]
        public void TestUDPLoggerChronology()
        {
            int countThreads = 3;
            logger = new Logger(2, new ILoggerTarget[] { new UDPLoggerTarget(portNumber, ipAddress) });
            Thread udpLogListenerThread = new Thread(new ParameterizedThreadStart(CheckPortLogChronology));
            Thread[] loggingThreads = new Thread[countThreads];
            ThreadManager.StartAllThreads(loggingThreads, TestThreadFunction);
            udpLogListenerThread.IsBackground = true;
            udpLogListenerThread.Priority = ThreadPriority.Highest;
            udpLogListenerThread.Start(portNumber);
            ThreadManager.WaitAllThreadsToFinish(loggingThreads);
            Thread.Sleep(2000);
        }

        private bool CheckFileLogChronology(String logFileName)
        {
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

        private void CheckPortLogChronology(Object portNumberObj)
        {
            int portNumber = (int)portNumberObj;
            UdpClient udpClient = new UdpClient(new IPEndPoint(ipAddress, portNumber));
            DateTime lastLogTime = default(DateTime);
            while(true)
            {
                IPEndPoint senderIpEndPoint = null;
                byte[] data = udpClient.Receive(ref senderIpEndPoint);
                ILogInfo[] logsInfo = ObjectConverters<ILogInfo[]>.ByteArrayConverter.TargetTypeToObject(data);//ByteArrayToObject(data);
                if(lastLogTime != default(DateTime))
                {
                    Assert.AreEqual(lastLogTime <= logsInfo[0].Time, true);
                }
                lastLogTime = logsInfo[0].Time;
                Console.WriteLine(lastLogTime);
            }
        }

    }

}