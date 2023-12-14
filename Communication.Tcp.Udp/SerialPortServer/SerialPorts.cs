//using System.IO.Ports;
//using System.Management;

//namespace Communication.Tcp.Udp.SerialPortServer
//{
//    /// <summary>
//    /// 串口帮助类
//    /// </summary>
//    public class SerialPorts
//    {
//        /// <summary>
//        /// 串口对象
//        /// </summary>
//        public SerialPort SerialPort;

//        public SerialPorts()
//        {
//            SerialPort = new SerialPort();
//            //SerialPort.DataReceived += new SerialDataReceivedEventHandler(ComSerialPort_DataReceived);
//        }

//        /// <summary>
//        /// 接收串口数据
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        //public void ComSerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
//        //{

//        //}

//        /// <summary>
//        /// 获取设备上所有串口
//        /// </summary>
//        /// <returns></returns>
//        public List<string> GetSerialPorts()
//        {
//            #region 获取所有串口

//            List<string> stringList = new();

//            string[] ports = SerialPort.GetPortNames();
//            ManagementObjectSearcher searcher = new("SELECT * FROM Win32_PnPEntity WHERE Caption LIKE '%(COM%'");

//            foreach (string port in ports)
//            {
//                foreach (ManagementObject queryObj in searcher.Get().Cast<ManagementObject>())
//                {
//                    if (queryObj["Caption"] != null)
//                    {
//                        string caption = queryObj["Caption"].ToString()!;
//                        if (caption.Contains(port))
//                        {
//                            stringList.Add(caption);
//                            break;
//                        }
//                    }
//                }
//            }

//            return stringList;
//            #endregion
//        }

//        /// <summary>
//        /// 打开串口
//        /// </summary>
//        /// <param name="PortName">串口号</param>
//        /// <param name="BaudRate">波特率</param>
//        /// <param name="Parity">奇偶校验</param>
//        /// <param name="DataBits">数据位</param>
//        /// <param name="StopBits">停止位</param>
//        /// <returns></returns>
//        public (bool, string) OpenSerialPort(string PortName, int BaudRate, Parity Parity, int DataBits, StopBits StopBits)
//        {
//            try
//            {
//                SerialPort.PortName = PortName;
//                SerialPort.BaudRate = BaudRate;
//                SerialPort.Parity = Parity;
//                SerialPort.DataBits = DataBits;
//                SerialPort.StopBits = StopBits;

//                SerialPort.Open();
//                return (true, "串口打开成功！");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                return (false, $"串口打开失败，异常：{ex.Message}");
//            }
//        }

//        /// <summary>
//        /// 下写数据
//        /// </summary>
//        /// <param name="buffer"></param>
//        /// <param name="offset"></param>
//        /// <param name="count"></param>
//        /// <returns></returns>
//        public (bool, string) Write(byte[] buffer, int offset, int count)
//        {
//            try
//            {
//                SerialPort.Write(buffer, offset, count);
//                return (true, "数据下写成功！");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                return (false, $"数据下写失败，异常：{ex.Message}");
//            }
//        }

//        /// <summary>
//        /// 关闭串口
//        /// </summary>
//        /// <returns></returns>
//        public (bool, string) CloseSerialPort()
//        {
//            try
//            {
//                SerialPort.Close();
//                return (true, "串口关闭成功！");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                return (false, $"串口关闭失败，异常：{ex.Message}");
//            }
//        }

//    }
//}
