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
using web_print_agent.Utils;
using System.Threading;

namespace web_print_agent
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
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
            PrintService pt = new PrintService();
            //配置socket服务
            mySocketServer = new MySocketService(pt);
            socketBase = new SocketBase(mySocketServer);
            socketBase.start();
            //获取打印机信息
            var printers = LocalPrinter.GetLocalPrinters();
            PrintList.ItemsSource = printers;
            PrintList.SelectedIndex = 0;
            MyLogService.Info("打印服务已启动");
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            windowsMin.onStateChange();
        }

        private void PrintList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string selectedEmployee = (string)comboBox.SelectedItem;
            if (!selectedEmployee.Equals(LocalPrinter.GetDefaultPrinter()))
            {
                try
                {
                    LocalPrinter.SetDefaultPrinter(selectedEmployee);
                    setMsg(Global.PRINTSETSUCESS+selectedEmployee);
                }
                catch (Exception ex)
                {
                    setMsg(ex.Message);
                }

            }
        }

        private void setMsg(string msg)
        {
            SystemMsg.Content = msg;
            Thread t = new Thread(() =>
            {
                Thread.Sleep(3000);//次线程休眠3秒
                Dispatcher.Invoke(new Action(() =>
                {
                    SystemMsg.Content = Global.SYSISRUN;
                }));
            });
            t.Start();
        }
    }
}
