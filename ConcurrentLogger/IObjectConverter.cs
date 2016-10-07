using System;

namespace ConcurrentLogger
{

    public interface IObjectConverter<T, E>
    {
        T TargetTypeToObject(E obj);
        E ObjectToTargetType(T obj);
    }

}