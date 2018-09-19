using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_print_agent.Service.Socket
{
    /// <summary>
    /// Socket服务类，此类只允许被继承，不可直接实例化
    /// </summary>
    public abstract class SocketService : ISocketService
    {
        public IDictionary<string, IWebSocketConnection> dic_Sockets = new Dictionary<string, IWebSocketConnection>();

        public virtual void OnConnected(string clientIp)
        {
        }

        public virtual void OnDisconnected()
        {
        }

        public abstract void OnError(Exception ex);

        public virtual void OnMessage(string msg, IWebSocketConnection client)
        {

        }

        public void sendAll(string msg)
        {
            foreach (var item in dic_Sockets)
            {
                item.Value.Send(msg);
            }
        }
    }
}
