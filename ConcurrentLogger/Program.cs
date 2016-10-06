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
            new Thread(firstThreadFunc).Start();
            new Thread(secondThreadFunc).Start();
            System.Console.ReadLine();
        }

        static void firstThreadFunc()
        {
            logger.Log(new LogInfo(LogLevel.INFO, "firstThread log 1"));
            logger.Log(new LogInfo(LogLevel.INFO, "firstThread log 2"));
        }

        static void secondThreadFunc()
        {
            logger.Log(new LogInfo(LogLevel.INFO, "secondThread log 1"));
            logger.Log(new LogInfo(LogLevel.INFO, "secondThread log 2"));
        }

    }

}