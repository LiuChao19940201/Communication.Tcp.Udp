using Communication.Tcp.Udp.SocketServers;
using Communication.Tcp.Udp.SocketServers.Sockets;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Threading;

namespace Communication.Tcp.Udp_TestDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Server tcp_ser = new();
        Client tcp_cli = new();
        UdpSoket udp_ser = new();

        private DispatcherTimer? timer;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            tcp_ser.EventInit();
            tcp_cli.EventInit();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            comboBox1.SelectedIndex = 1;

            // 初始化定时器
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick; // 绑定定时器Tick事件处理程序
            timer.Start(); // 启动定时器
        }


        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (Res.formname is not null)
                this.Title = Res.formname;

            if (Res.log.Count > 0)
            {
                textBox2.Text += Res.log[0] + "\r\n";
                Res.log.RemoveAt(0);
            }
            if (Res.TcpLog.Count > 0)
            {
                textBox2.Text += Res.TcpLog[0];
                Res.TcpLog.RemoveAt(0);
            }

            // 更新客户端连接信息
            if (Res.OpenState && comboBox1.SelectedIndex == 1)
            {
                List<string> ips = tcp_ser?.GetIps()!;
                ips.Add("ALL");
                if (ips.Count > 0)
                {
                    foreach (string s in ips)
                    {
                        int a = comboBox2.Items.IndexOf(s);
                        if (a == -1) comboBox2.Items.Add(s);
                    }
                    if (comboBox2.Text == "") comboBox2.SelectedIndex = 0;
                }
            }
            else
            {
                comboBox2.Items.Clear();
            }
            tcp_ser!.cIp = comboBox2.Text;
        }

        /// <summary>
        /// 打开网络
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            // UDP server
            if (comboBox1.SelectedIndex == 0)
            {
                udp_ser.s_port = int.Parse(textBox1.Text);
                udp_ser.s_ip = comboBox3.Text;
                udp_ser.StartReceive();
            }

            // TCP server
            if (comboBox1.SelectedIndex == 1)
            {
                tcp_ser.port = int.Parse(textBox1.Text);
                tcp_ser.ip = comboBox3.Text;
                tcp_ser.OpenCloseServer();
            }

            // TCP client
            if (comboBox1.SelectedIndex == 2)
            {
                tcp_cli.port = int.Parse(textBox1.Text);
                tcp_cli.ip = comboBox3.Text;
                tcp_cli.OpenCloseClient();
            }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string data = textBox3.Text;

            if (comboBox1.SelectedIndex == 0)
            {
                udp_ser.SendData(System.Text.Encoding.Default.GetBytes(data));
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                tcp_ser.SendData(tcp_ser.cIp, System.Text.Encoding.Default.GetBytes(data));
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                tcp_cli.SendData(System.Text.Encoding.Default.GetBytes(data));
            }
        }
        private void comboBox1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (label14 is null)
            {
                return;
            }
            if (comboBox1.SelectedIndex == 0)
            {
                label14.Text = "本地主机地址";
                label13.Text = "本地主机端口";
                comboBox3.Items.Clear();
                comboBox3.Items.Add("127.0.0.1");
                comboBox3.Items.Add(GetLocalIP());
                comboBox3.SelectedIndex = 0;
                comboBox3.IsEditable = true;

                comboBox2.Items.Clear();
                comboBox2.Items.Add("127.0.0.1:8080");
                comboBox2.Items.Add(GetLocalIP() + ":8080");
                comboBox2.SelectedIndex = 0;
                comboBox2.IsEditable = true;
            }
            if (comboBox1.SelectedIndex == 1)
            {
                label14.Text = "本地主机地址";
                label13.Text = "本地主机端口";
                comboBox3.Items.Clear();
                comboBox3.Items.Add("127.0.0.1");
                comboBox3.Items.Add(GetLocalIP());
                comboBox3.Items.Add("ALL");
                comboBox3.SelectedIndex = 0;
                comboBox3.IsEditable = true;

                comboBox2.Items.Clear();
                comboBox2.Text = "";
                comboBox2.IsEditable = true;
            }
            if (comboBox1.SelectedIndex == 2)
            {
                label14.Text = "远程主机地址";
                label13.Text = "远程主机端口";
                comboBox3.Items.Clear();
                comboBox3.Items.Add("127.0.0.1");
                comboBox3.Items.Add(GetLocalIP());
                comboBox3.SelectedIndex = 0;
                comboBox3.IsEditable = true;

                comboBox2.Items.Clear();
                comboBox2.Text = "";
                comboBox2.IsEditable = true;
            }
        }

        public string GetLocalIP()
        {
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        string strip = IpEntry.AddressList[i].ToString();

                        string[] sArray = strip.Split('.');
                        if (sArray[3] != "1")
                            return IpEntry.AddressList[i].ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                Res.log.Add(ex.Message + "\r\n");
                return "";
            }
        }

        private void comboBox1_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}