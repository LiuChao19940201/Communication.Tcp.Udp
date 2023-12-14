using System.Net;
using System.Net.Sockets;

namespace Communication.Tcp.Udp.SocketServers.Sockets
{
    public class UdpSoket
    {
        /// <summary>
        /// 用于UDP发送的网络服务类
        /// </summary>
        static UdpClient? udpcRecvSend = null;

        static IPEndPoint? localIpep = null;

        /// <summary>
        /// 开关：在监听UDP报文阶段为true，否则为false
        /// </summary>
        static bool IsUdpcRecvStart = false;

        /// <summary>
        /// 线程：不断监听UDP报文
        /// </summary>
        static Thread? thrRecv;

        public int s_port = 8080;
        public string s_ip = "127.0.0.1";

        public int c_port = 8080;
        public string c_ip = "127.0.0.1";

        public void StartReceive()
        {
            if (!Res.OpenState)
            {
                Res.OpenState = true;
                Res.formname += " [UDP " + s_ip + ":" + s_port + "]";
                if (!IsUdpcRecvStart) // 未监听的情况，开始监听
                {
                    localIpep = new IPEndPoint(IPAddress.Parse(s_ip), s_port); // 本机IP和监听端口号
                    udpcRecvSend = new UdpClient(localIpep);
                    thrRecv = new Thread(ReceiveMessage);
                    thrRecv.Start();
                    IsUdpcRecvStart = true;
                    Console.WriteLine("UDP监听器已成功启动");
                }
            }
            else
            {
                Res.formname = "";
                Res.OpenState = false;
                StopReceive();
            }
        }

        public void StopReceive()
        {
            if (IsUdpcRecvStart)
            {
#pragma warning disable SYSLIB0006
                // 忽略警告:你的代码，其中可能包含已过时的Thread.Abort()调用  
                thrRecv?.Abort(); // 必须先关闭这个线程，否则会异常
#pragma warning restore SYSLIB0006

                udpcRecvSend?.Close();
                IsUdpcRecvStart = false;
                Console.WriteLine("UDP监听器已成功关闭");
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="obj"></param>
        private void ReceiveMessage(object? obj)
        {
            while (IsUdpcRecvStart)
            {
                try
                {
                    byte[] data = udpcRecvSend?.Receive(ref localIpep)!;

                    string hexOutput = BitConverter.ToString(data);
                    hexOutput = hexOutput.Replace("-", " ");
                    Res.TcpLog.Add("接收:" + hexOutput + "\r\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
            }
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="obj"></param>
        public void SendData(Byte[] data)
        {
            try
            {
                IPEndPoint remoteIpep = new IPEndPoint(IPAddress.Parse(c_ip), c_port); // 发送到的IP地址和端口号
                udpcRecvSend?.Send(data, data.Length, remoteIpep);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
