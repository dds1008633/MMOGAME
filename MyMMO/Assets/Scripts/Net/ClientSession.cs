using LogicProtocol;
using Net;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncNetExampleClient
{
    public class ClientSession : AsyncSession
    {
        protected override void OnConnected(bool result)
        {
            AsyncTool.Log("Connect server:{0}", result);
        }

        protected override void OnDisconnected()
        {
            AsyncTool.Log("DisConnect to Server.");
        }

        protected override void OnReceiveMsg(Pkg msg)
        {
            AsyncTool.Log("ReceiveClientMsg:-----");
            Pkg pack = msg as Pkg;
            switch (pack.Head.Cmd)
            {
                case Cmd.LogicLogin:                  
                    break;
                case Cmd.BagInfo:
                    break;
                default:
                    break;
            }
        }

        public void ReqPBLogin()
        {
            Pkg pkg = new Pkg
            {
                Head = new Head
                {
                    Cmd = Cmd.LogicLogin,
                    Seq = 1
                },
                Body = new Body
                {
                    reqLogicLogin = new ReqLogicLogin
                    {
                        Acct = "Plane",
                        Pass = "www.qiqiker.com"
                    }
                }
            };     
            SendMsg(pkg);
        }
    }
}
