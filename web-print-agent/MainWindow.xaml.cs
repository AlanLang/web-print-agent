using System;
using Arthas.Controls.Metro;
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
using web_print_agent.Utils;
using System.Threading;

namespace web_print_agent
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        SocketBase socketBase;
        MySocketService mySocketServer;
        WindowsMin windowsMin;
        public MainWindow()
        {
            InitializeComponent();
            if (Global.canSatrt)
            {
                init();
            }
        }

        private void init()
        {
            windowsMin = new WindowsMin(this);// 设置最小化到任务栏
            //配置socket服务
            mySocketServer = new MySocketService();
            socketBase = new SocketBase(mySocketServer);
            socketBase.start();
            //获取打印机信息
            var pts = LocalPrinter.GetLocalPrinters();
            MyLogService.Info("打印服务已启动");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mySocketServer.sendAll("发送测试");
            int i = socketBase.connectNum;
            string s = "";
            s.Substring(1, 23);

        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Global.canClose = true;
            windowsMin.hide();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            windowsMin.onStateChange();
        }
    }
}
