namespace Communication.Tcp.Udp.SocketServers
{
    public static class Res
    {
        /// <summary>
        /// 设备打开状态
        /// </summary>
        public static bool OpenState { get; set; }

        /// <summary>
        /// 页面提示信息
        /// </summary>
        public static string? formname { get; set; }

        /// <summary>
        /// 日志
        /// </summary>
        public static List<string> log = new List<string>();

        /// <summary>
        /// TCP收发数据
        /// </summary>
        public static List<string> TcpLog = new List<string>();
    }
}
