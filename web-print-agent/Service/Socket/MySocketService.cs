using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_print_agent.Service.Socket
{
    public class MySocketService:SocketService
    {
        public override void OnError(Exception ex)
        {
            MyLogService.Error("socket调用发生错误：" + ex.Message);
        }
    }
}
