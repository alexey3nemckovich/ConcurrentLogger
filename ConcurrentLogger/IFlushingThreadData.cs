namespace ConcurrentLogger
{

    public interface IFlushingThreadData<T>
    {
        ILogInfo[] LogsInfo { get; }
        T Data { get; }
    }

}