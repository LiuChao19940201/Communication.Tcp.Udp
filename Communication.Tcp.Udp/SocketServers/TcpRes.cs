using Communication.Tcp.Udp.SocketServers.Sockets;

namespace Communication.Tcp.Udp.SocketServers
{
    public static class TcpRes
    {
        /// <summary>
        /// 服务器
        /// </summary>
        public static TcpSocketServer? TSS { get; set; }

        /// <summary>
        /// 客户端
        /// </summary>
        public static TcpSocketClient? TSC { get; set; }

        /// <summary>
        /// socket 连接池
        /// </summary>
        public static List<Machine>? Socs { get; set; } = new List<Machine>();

        /// <summary>
        /// 记录反馈信息
        /// </summary>
        public static Action<TcpSocketConnection, byte[]>? TcpServerRecv = null;
        public static Action<byte[]>? TcpClientRecv = null;

        /// <summary>
        /// 缓存设备信息
        /// </summary>
        /// <param name="p"></param>
        public static void UpMachine(TcpSocketConnection s)
        {
            Machine equipment = new Machine();
            equipment.TSS = s;
            equipment.ClientIP = s.GetClientIP();
            Machine? m = Socs?.FirstOrDefault(x => x.TSS == s);
            if (m == null)
            {
                Socs?.Add(equipment);
            }
            else
            {
                m.ClientIP = s.GetClientIP();
            }
        }
    }
}
