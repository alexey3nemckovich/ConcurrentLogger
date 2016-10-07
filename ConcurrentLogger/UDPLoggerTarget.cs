using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConcurrentLogger
{

    public class UDPLoggerTarget : ILoggerTarget
    {
        
        private IPEndPoint ipEndPoint;
        private UdpClient udpClient;

        public UDPLoggerTarget() : this(50000)
        {

        }

        public UDPLoggerTarget(int portNumber) : this(portNumber, IPAddress.Any)
        {

        }

        public UDPLoggerTarget(int portNumber, IPAddress ipAddress)
        {
            this.ipEndPoint = new IPEndPoint(ipAddress, portNumber);
        }

        public bool Flush(ILogInfo[] logsInfo)
        {
            byte[] logsInfoBytes = ObjectConverters<ILogInfo[]>.ByteArrayConverter.ObjectToTargetType(logsInfo);//objectToByteArray(logsInfo);
            using (UdpClient udpClient = new UdpClient())
            {
                try
                {
                    udpClient.Connect(ipEndPoint);
                    udpClient.Send(logsInfoBytes, logsInfoBytes.Length);
                }
                finally
                {
                    udpClient.Close();
                }
            }
            return true;
        }

        private bool Flush(Object logsInfoObj)
        {
            ILogInfo[] logsInfo = (ILogInfo[])logsInfoObj;
            return Flush(logsInfo);
        }

        public Task<bool> FlushAsync(ILogInfo[] logsInfo)
        {
            Task<bool> flushTask = new Task<bool>(Flush, logsInfo);
            flushTask.Start();
            return flushTask;
        }

    }

}