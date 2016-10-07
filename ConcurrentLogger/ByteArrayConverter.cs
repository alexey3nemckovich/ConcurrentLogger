using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConcurrentLogger
{

    class ByteArrayConverter<T, E> : IObjectConverter<T, E>
    {
        
        public T TargetTypeToObject(E targetTypeObj)
        {
            byte[] byteArray = (byte[])(Object)targetTypeObj;
            if (byteArray.Length == 0)
            {
                return default(T);
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(byteArray, 0, byteArray.Length);
                memoryStream.Position = 0;
                return (T)binaryFormatter.Deserialize(memoryStream);
            }
        }

        public E ObjectToTargetType(T obj)
        {
            if (obj == null)
            {
                return default(E);
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                return (E)(Object)memoryStream.ToArray();
            }
        }

    }

}