using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace web_print_agent.Utils
{
    public class WindowsMin
    {
        WindowState wsl;
        System.Windows.Forms.NotifyIcon notifyIcon;
        Window windows;
        public WindowsMin(Window windows)
        {
            setMin();
            this.windows = windows;
            wsl = this.windows.WindowState;
        }

        public void hide()
        {
            this.notifyIcon.Visible = false;
        }

        public void onStateChange()
        {
            WindowState ws = this.windows.WindowState;
            if (ws == WindowState.Minimized)
            {
                this.windows.Hide();
            }
        }

        private void setMin()
        {
            this.notifyIcon = new System.Windows.Forms.NotifyIcon();
            this.notifyIcon.BalloonTipText = "欢迎使用打印服务"; //设置程序启动时显示的文本
            this.notifyIcon.Text = "打印服务";//最小化到托盘时，鼠标点击时显示的文本
            this.notifyIcon.Icon = new System.Drawing.Icon("print.ico");//程序图标
            this.notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            this.notifyIcon.Visible = true;

            //右键菜单--打开菜单项
            System.Windows.Forms.MenuItem open = new System.Windows.Forms.MenuItem("打开");
            open.Click += new EventHandler(ShowWindow);
            //右键菜单--退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("关闭");
            exit.Click += new EventHandler(CloseWindow);
            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { open, exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            notifyIcon.MouseDoubleClick += ShowWindow;
            this.notifyIcon.ShowBalloonTip(1000);
        }


        private void ShowWindow(object sender, EventArgs e)
        {
            this.windows.Show();
            this.windows.WindowState = wsl;
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
