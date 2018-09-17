using Sodao.FastSocket.Server;
using Sodao.FastSocket.Server.Command;
using Sodao.FastSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_print_agent.Service
{
    public class MySocketService : CommandSocketService<AsyncBinaryCommandInfo>
    {
        protected override void HandleUnKnowCommand(IConnection connection, AsyncBinaryCommandInfo commandInfo)
        {
            throw new NotImplementedException();
        }
    }
}
