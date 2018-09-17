using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using web_print_agent.Service;

namespace web_print_agent
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MyLogService.Info("打印代理服务已启动");
            var server = new WebSocketServer("ws://0.0.0.0:9401");
            server.RestartAfterListenError = true;
            server.Start(socket =>
            {
                socket.OnMessage = message => socket.Send("接收到消息"+message);
                //...use as normal
            });
        }
    }
}
