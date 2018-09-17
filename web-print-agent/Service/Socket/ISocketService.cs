using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_print_agent.Service.Socket
{
    public interface ISocketService
    {

        /// <summary>
        /// 当建立socket连接时，会调用此方法
        /// </summary>
        /// <returns></returns>
        void OnConnected(string clientIp);

        void OnDisconnected();

        void OnMessage(string msg, IWebSocketConnection client);

        void OnError(Exception ex);
    }
}
