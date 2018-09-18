using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using web_print_agent.Utils;

namespace web_print_agent
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            int i = e.Args.Length;
            log4net.Config.XmlConfigurator.Configure();

            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            base.OnStartup(e);
        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }

        //主线程异常
        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                e.Handled = true;
                Service.MyLogService.Error("未捕获的主线程异常" + e.Exception.Message, e.Exception);
                MessageBox.Show(e.Exception.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("应用程序发生不可恢复的异常，将要退出！");
            }

        }

        //非主线程异常
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Service.MyLogService.Error("未捕获的子线程异常", e.ExceptionObject as Exception);
        }
    }
}
