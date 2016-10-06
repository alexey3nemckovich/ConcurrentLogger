using System.Threading;

namespace TestConcurrentLogger
{

    public static class ThreadManager
    {

        public static void ExecuteAndWaitAllThreads(int threadsCount, ThreadStart threadFunction)
        {
            Thread[] threads = new Thread[threadsCount];
            InitAllThreads(threads, threadFunction);
            StartAllThreads(threads);
            WaitAllThreadsToFinish(threads);
        }

        public static void InitAllThreads(Thread[] testThreads, ThreadStart threadStart)
        {
            int threadsCount = testThreads.Length;
            for (int i = 0; i < threadsCount; i++)
            {
                testThreads[i] = new Thread(threadStart);
            }
        }

        public static void StartAllThreads(Thread[] testThreads)
        {
            int threadsCount = testThreads.Length;
            for (int i = 0; i < threadsCount; i++)
            {
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