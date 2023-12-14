using Communication.Tcp.Udp.SocketServers.Sockets;

namespace Communication.Tcp.Udp.SocketServers
{
    public class Server
    {
        public int port = 8080;
        public string ip = "127.0.0.1";

        public string cIp = "";

        /// <summary>
        /// 初始化设备
        /// </summary>
        public void OpenCloseServer()
        {
            if (!Res.OpenState)
            {
                Res.OpenState = true;
                Res.formname += " [TCP Server " + ip + ":" + port + "]";
                TcpRes.TSS = new TcpSocketServer(ip, port);
                TcpRes.TSS.HandleRecMsg = Calculation;
                TcpRes.TSS.HandleClientClose = ClientDisconnect;
                TcpRes.TSS.StartServer();
            }
            else
            {
                Res.formname = "";
                Res.OpenState = false;
                TcpRes.TSS!.StopServer();
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="package"></param>
        public void Calculation(TcpSocketServer t, TcpSocketConnection s, byte[] package)
        {
            try
            {
                TcpRes.TcpServerRecv?.Invoke(s, package);
            }
            catch (Exception e)
            {
                Res.log.Add(e.Message + "\r\n");
            }
        }

        /// <summary>
        /// 客户端断开连接
        /// </summary>
        public void ClientDisconnect(TcpSocketServer s, TcpSocketConnection c)
        {
            List<Machine> v = TcpRes.Socs!.FindAll(d => d.TSS == c);
            if (v.Count > 0) v.ForEach(f => f.ClientIP = "");
            TcpRes.Socs.RemoveAll(x => x.ClientIP == "");
        }

        /// <summary>
        ///  发送数据
        /// </summary>
        public void SendData(string ip, byte[] data)
        {
            try
            {
                if (data == null || ip == "") return;
                if (ip == "ALL")
                {
                    foreach (Machine m in TcpRes.Socs!)
                    {
                        m.TSS?.Send(data);
                    }
                }
                else
                {
                    Machine m = TcpRes.Socs?.FirstOrDefault(x => x.ClientIP == ip)!;
                    if (m != null && m.TSS != null) m.TSS.Send(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 获取IP列表
        /// </summary>
        /// <returns></returns>
        public List<string>? GetIps()
        {
            try
            {
                List<string> ips = TcpRes.Socs!.Select(x => x.ClientIP).ToList()!;
                return ips;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// 事件初始化
        /// </summary>
        public void EventInit()
        {
            TcpRes.TcpServerRecv = (s, data) =>
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


