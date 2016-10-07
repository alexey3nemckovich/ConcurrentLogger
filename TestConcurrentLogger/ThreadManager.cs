using System.Threading;

namespace TestConcurrentLogger
{

    public static class ThreadManager
    {

        public static void StartAndWaitAllThreads(Thread[] threads, ThreadStart threadFunction)
        {
            StartAllThreads(threads, threadFunction);
            WaitAllThreadsToFinish(threads);
        }

        public static void StartAllThreads(Thread[] threads, ThreadStart threadStart)
        {
            int threadsCount = threads.Length;
            for (int i = 0; i < threadsCount; i++)
            {
                threads[i] = new Thread(threadStart);
                threads[i].Start();
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

        public static bool AllThreadsFinished(Thread[] threads)
        {
            int threadsCount = threads.Length;
            for (int i = 0; i < threadsCount; i++)
            {
                if(threads[i].IsAlive)
                {
                    return false;
                }
            }
            return true;
        }

    }

}