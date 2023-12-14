using Communication.Tcp.Udp.SocketServers.Sockets;

namespace Communication.Tcp.Udp.SocketServers
{
    public class Client
    {
        public int port = 8080;
        public string ip = "";

        /// <summary>
        /// 初始化设备
        /// </summary>
        public void OpenCloseClient()
        {
            if (!Res.OpenState)
            {
                Res.OpenState = true;
                Res.formname += " [TCP Client " + ip + ":" + port + "]";
                new Thread(StartClientThread) { IsBackground = true }.Start();
            }
            else
            {
                Res.formname = "";
                Res.OpenState = false;
                TcpRes.TSC!.StopClient();
            }
        }

        private void StartClientThread()
        {
            TcpRes.TSC = new TcpSocketClient(ip, port);
            TcpRes.TSC.HandleRecMsg = Calculation;
            TcpRes.TSC.HandleClientClose = ServerDisconnect;
            TcpRes.TSC.StartClient();
        }

        /// <summary>
        /// 反构造数据包并解析
        /// </summary>
        /// <param name="package"></param>
        public void Calculation(TcpSocketClient t, byte[] package)
        {
            try
            {
                TcpRes.TcpClientRecv?.Invoke(package);
            }
            catch (Exception e)
            {
                Res.log.Add(e.Message + "\r\n");
            }
        }

        /// <summary>
        ///  发送数据
        /// </summary>
        public void SendData(byte[] data)
        {
            try
            {
                TcpRes.TSC!.Send(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 服务端断开连接
        /// </summary>
        public void ServerDisconnect(TcpSocketClient s)
        {
            Res.OpenState = false;
            Res.log.Add("服务器断开连接\r\n");
            s.StopClient();
        }

        /// <summary>
        /// 接收数据事件初始化
        /// </summary>
        public void EventInit()
        {
            TcpRes.TcpClientRecv = (data) =>
            {
                try
                {
                    if (data.Length > 0)
                    {
                        string hexOutput = BitConverter.ToString(data);
                        hexOutput = hexOutput.Replace("-", " ");
                        Res.TcpLog.Add("接收:" + hexOutput + "\r\n");
                    }
                }
                catch (Exception e)
                {
                    Res.log.Add(e.Message + "\r\n");
                }
            };
        }
    }
}
