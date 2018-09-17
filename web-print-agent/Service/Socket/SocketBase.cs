using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_print_agent.Service.Socket
{
    public class SocketBase
    {
        SocketService socketService;

        public SocketBase(SocketService socketService) {
            this.socketService = socketService;
        }

        /// <summary>
        /// 当前连接数
        /// </summary>
        public int connectNum {
            get { return this.socketService.dic_Sockets.Count; }
        }

        public void start() {
            var server = new WebSocketServer("ws://0.0.0.0:9401");
            server.RestartAfterListenError = true;
            server.Start(socket =>
            {
                socket.OnOpen = () =>   //连接建立事件
                {
                    //获取客户端网页的url
                    string clientUrl = socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort;
                    this.socketService.dic_Sockets.Add(clientUrl, socket);
                    this.socketService.OnConnected(clientUrl);
                };
                socket.OnMessage = message => {
                        this.socketService.OnMessage(message, socket);
                    };
                socket.OnClose = () =>  //连接关闭事件
                {
                    string clientUrl = socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort;
                    //如果存在这个客户端,那么对这个socket进行移除
                    if (this.socketService.dic_Sockets.ContainsKey(clientUrl))
                    {
                        this.socketService.dic_Sockets.Remove(clientUrl);
                    }
                    this.socketService.OnDisconnected();
                };
                socket.OnError = (ex) =>
                {
                    this.socketService.OnError(ex);
                };
            });
        }
    }
}
