using System.Threading;

namespace TestConcurrentLogger
{

    public static class ThreadManager
    {

        public static void ExecuteAndWaitAllThreads(int threadsCount, ThreadStart threadFunction)
        {
            Thread[] threads = new Thread[threadsCount];
            InitAndStartAllThreads(threads, threadFunction);
            WaitAllThreadsToFinish(threads);
        }

        public static void InitAndStartAllThreads(Thread[] testThreads, ThreadStart threadStart)
        {
            int threadsCount = testThreads.Length;
            for (int i = 0; i < threadsCount; i++)
            {
                testThreads[i] = new Thread(threadStart);
                testThreads[i].Start();
            }
        }

        public static void WaitAllThreadsToFinish(Thread[] threads)
        {
            int threadsCount = threads.Length;
            for (int i = 0; i < threadsCount; i++)
            {
                threads[i].Join();
            }
        }

    }

}