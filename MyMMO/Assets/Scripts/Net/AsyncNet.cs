using AsyncNetExampleClient;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Net
{  
    public class AsyncNet:SingletonMono<AsyncNet>
    {
        public ClientSession session;        
        private Socket socket = null;
        public int backlog = 10;
        private AsyncCallback serverConnectCallback;
       
        #region Client
        public void StartClient(string ip, int port)
        {
            try
            {
                serverConnectCallback = ServerConnectCB;
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                AsyncTool.Log("Client Start...");
                EndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                socket.BeginConnect(endPoint, serverConnectCallback, null);
            }
            catch (Exception e)
            {
                AsyncTool.Log(e.Message + e.StackTrace);
            }
        }

        private void ServerConnectCB(IAsyncResult ar)
        {
            session = new ClientSession();
            try
            {
                socket.EndConnect(ar);
                if (socket.Connected)
                {
                    session.InitSession(socket, null);
                }
            }
            catch (Exception e)
            {
                AsyncTool.Log("ServerConnectCB Error:{0}", e.Message);

            }
        }
        public void CloseClient()
        {
            if (session != null)
            {
                session.CloseSession();
                session = null;
            }
            if (socket != null)
            {
                socket = null;
            }
        }
        #endregion
    }      

}
