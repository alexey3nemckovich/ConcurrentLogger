using System.Threading;

namespace ConcurrentLogger
{

    class Program
    {

        static Logger logger;

        static void Main(string[] args)
        {
            LoggerTarget[] targets = new LoggerTarget[3];
            for(int i = 0; i < targets.Length; i++)
            {
                targets[i] = new LoggerTarget();
            }
            int bufferLimit = 2;
            logger = new Logger(bufferLimit, targets);
            Thread[] threads = new Thread[10];
            for (int i = 0; i < 10; i++)
            {
                threads[i] = new Thread(ThreadLogFunc);
            }
            Thread.Sleep(300);
            for (int i = 0; i < 10; i++)
            {
                threads[i].Start();
            }
            System.Console.ReadLine();
        }

        static void ThreadLogFunc()
        {
            for(int i = 0; i < 4; i++)
            {
                logger.Log(new LogInfo(LogLevel.INFO, "task " + i + " of thread №" + Thread.CurrentThread.ManagedThreadId + " Start"));
                Thread.Sleep(1000);
                logger.Log(new LogInfo(LogLevel.INFO, "task " + i + " of thread №" + Thread.CurrentThread.ManagedThreadId + " End"));
            }
        }

    }

}