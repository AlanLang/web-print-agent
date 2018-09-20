using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace web_print_agent
{
    /// <summary>
    /// NotificationWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NotificationWindow : Window
    {
        public double TopFrom
        {
            get; set;
        }
        public NotificationWindow()
        {
            InitializeComponent();
            this.Loaded += NotificationWindow_Loaded;
        }

        private void NotificationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            NotifyData data = this.DataContext as NotifyData;
            if (data != null)
            {
                tbContent.Text = data.Content;
                tbTitle.Text = data.Title;
            }
            NotificationWindow self = sender as NotificationWindow;
            if (self != null)
            {
                self.UpdateLayout();
                //SystemSounds.Asterisk.Play();//播放提示声

                double right = System.Windows.SystemParameters.WorkArea.Right;//工作区最右边的值
                self.Top = self.TopFrom - self.ActualHeight;
                DoubleAnimation animation = new DoubleAnimation();
                animation.Duration = new Duration(TimeSpan.FromMilliseconds(500));//NotifyTimeSpan是自己定义的一个int型变量，用来设置动画的持续时间
                animation.From = right;
                animation.To = right - self.ActualWidth;//设定通知从右往左弹出
                self.BeginAnimation(Window.LeftProperty, animation);//设定动画应用于窗体的Left属性

                Task.Factory.StartNew(delegate
                {
                    int seconds = 5;//通知持续5s后消失
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(seconds));
                    //Invoke到主进程中去执行
                    this.Dispatcher.Invoke(delegate
                    {
                        animation = new DoubleAnimation();
                        animation.Duration = new Duration(TimeSpan.FromMilliseconds(500));
                        animation.Completed += (s, a) => { self.Close(); };//动画执行完毕，关闭当前窗体
                        animation.From = right - self.ActualWidth;
                        animation.To = right;//通知从左往右收回
                        self.BeginAnimation(Window.LeftProperty, animation);
                    });
                });
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double right = System.Windows.SystemParameters.WorkArea.Right;
            DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(500));

            animation.Completed += (s, a) => { this.Close(); };
            animation.From = right - this.ActualWidth;
            animation.To = right;
            this.BeginAnimation(Window.LeftProperty, animation);
        }
    }

    class NotifyData
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
