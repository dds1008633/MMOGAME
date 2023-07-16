using AsyncExampleProtocol;
using Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncNetExampleClient
{
    public class ClientSession : AsyncSession<NetMsg>
    {
        protected override void OnConnected(bool result)
        {
            AsyncTool.Log("Connect server:{0}", result);
        }

        protected override void OnDisconnected()
        {
            AsyncTool.Log("DisConnect to Server.");
        }

        protected override void OnReceiveMsg(NetMsg msg)
        {
            AsyncTool.Log("ReceiveServerMsg:{0}", msg.hellomsg);
        }
    }
}
