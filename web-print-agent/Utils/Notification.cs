using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_print_agent.Utils
{
    public class Notification
    {
        public static List<NotificationWindow> _dialogs = new List<NotificationWindow>();
        public void Open(string title,string content)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                NotifyData data = new NotifyData();
                data.Title = title;
                data.Content = content;
                NotificationWindow dialog = new NotificationWindow();//new 一个通知
                dialog.Closed += Dialog_Closed;
                dialog.TopFrom = GetTopFrom();
                _dialogs.Add(dialog);
                dialog.DataContext = data;//设置通知里要显示的数据
                dialog.Show();
            }));
        }
        private void Dialog_Closed(object sender, EventArgs e)
        {
            var closedDialog = sender as NotificationWindow;
            _dialogs.Remove(closedDialog);
        }
        double GetTopFrom()
        {
            //屏幕的高度-底部TaskBar的高度。
            double topFrom = System.Windows.SystemParameters.WorkArea.Bottom - 10;
            bool isContinueFind = _dialogs.Any(o => o.TopFrom == topFrom);

            while (isContinueFind)
            {
                topFrom = topFrom - 110;//此处100是NotifyWindow的高 110-100剩下的10  是通知之间的间距
                isContinueFind = _dialogs.Any(o => o.TopFrom == topFrom);
            }

            if (topFrom <= 0)
                topFrom = System.Windows.SystemParameters.WorkArea.Bottom - 10;

            return topFrom;
        }
    }
}
