namespace ConcurrentLogger
{

    public class ObjectConverters<T>
    {

        private static IObjectConverter<T, byte[]> byteArrayConverter = new ByteArrayConverter<T, byte[]>();

        public static IObjectConverter<T, byte[]> ByteArrayConverter
        {
            get
            {
                return byteArrayConverter;
            }
        }
        
    }

}