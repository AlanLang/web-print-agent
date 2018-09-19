using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fleck;

namespace web_print_agent.Service.Socket
{
    public class MySocketService:SocketService
    {
        PrintService printService;
        public MySocketService(PrintService printService)
        {
            this.printService = printService;
        }
        public override void OnError(Exception ex)
        {
            MyLogService.Error("socket调用发生错误：" + ex.Message);
        }

        public override void OnMessage(string msg, IWebSocketConnection client)
        {
            printService.SetPrintOrder(msg);
            printService.TestPrintOrder();
            base.OnMessage(msg, client);
        }
    }
}
