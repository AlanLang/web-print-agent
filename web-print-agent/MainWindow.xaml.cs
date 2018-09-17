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
using web_print_agent.Service.Socket;

namespace web_print_agent
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        SocketBase socketBase;
        MySocketService mySocketServer;
        public MainWindow()
        {
            InitializeComponent();
            MyLogService.Info("打印代理服务已启动");
            mySocketServer = new MySocketService();
            socketBase = new SocketBase(mySocketServer);
            socketBase.start();
            var pts =  LocalPrinter.GetLocalPrinters();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mySocketServer.sendAll("发送测试");
            int i = socketBase.connectNum;
        }
    }
}
