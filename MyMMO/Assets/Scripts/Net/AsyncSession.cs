using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using LogicProtocol;
using ProtoBuf;
namespace Net
{

    public enum AsyncSessionState
    {
        NONE,
        CONNECTED,
        NONCONNECTED
    }

    /// <summary>
    /// 网络会话，数据收发
    /// </summary>
    public abstract class AsyncSession
    {
        private Socket socket;
        private AsyncCallback receiveHeadCallback;
        private AsyncCallback receiveBodyCallback;
        private AsyncCallback sendCallback;
        private Action closeCallback;
        public AsyncSessionState sessionSate = AsyncSessionState.NONE;
        public void InitSession(Socket socket, Action closeCB)
        {
            receiveHeadCallback = ReceiveHeadData;
            receiveBodyCallback = RecevieBodyData;
            sendCallback = SendCB;
            closeCallback = closeCB;
            bool result = false;
            try
            {
                this.socket = socket;
                AsyncPackage pack = new AsyncPackage();
                socket.BeginReceive(
                    pack.headBuff,
                    0,
                    AsyncPackage.headLen,
                    SocketFlags.None,
                    receiveHeadCallback,
                    pack);
                result = true;
                sessionSate = AsyncSessionState.CONNECTED;
            }
            catch (Exception e)
            {
                AsyncTool.Log(e.Message + e.StackTrace);
            }
            finally
            {
                OnConnected(result);
            }
        }

        private void ReceiveHeadData(IAsyncResult ar)
        {
            try
            {
                if (socket == null || !socket.Connected)
                {
                    AsyncTool.Log("Socket is null or not connected!");
                    return;
                }
                AsyncPackage pack = (AsyncPackage)ar.AsyncState;
                int len = socket.EndReceive(ar);
                if (len == 0)
                {
                    AsyncTool.Log("远程连接下线...");
                    CloseSession();
                    return;
                }

                pack.headIndex += len;
                if (pack.headIndex < AsyncPackage.headLen) //数据头循环处理
                {
                    socket.BeginReceive(
                        pack.headBuff,
                        pack.headIndex,
                        AsyncPackage.headLen - pack.headIndex,
                        SocketFlags.None,
                        receiveHeadCallback,
                        pack
                        );
                }
                else //数据体循环处理
                {
                    pack.InitBodyBuff();
                    socket.BeginReceive(
                        pack.bodyBuff,
                        0,
                        pack.bodyLen,
                        SocketFlags.None,
                        receiveBodyCallback,
                        pack
                        );
                }

            }
            catch (Exception e)
            {
                AsyncTool.Log("ReceiveHeadData:" + e.Message);
                CloseSession();
            }
        }

        private void RecevieBodyData(IAsyncResult ar)
        {
            try
            {
                if (socket == null || !socket.Connected)
                {
                    AsyncTool.Log("Socket is null or not connected!");
                    return;
                }
                AsyncPackage pack = (AsyncPackage)ar.AsyncState;
                int len = socket.EndReceive(ar);
                if (len == 0)
                {
                    AsyncTool.Log("远程连接下线...");
                    CloseSession();
                    return;
                }
                pack.bodyIndex += len;
                if (pack.bodyIndex < pack.bodyLen) //数据体循环处理
                {
                    socket.BeginReceive(
                        pack.bodyBuff,
                        pack.bodyIndex,
                        pack.bodyLen - pack.bodyIndex,
                        SocketFlags.None,
                        receiveBodyCallback,
                        pack
                        );
                }
                else //数据头循环处理
                {
                    //反序列化，处理网络消息的业务逻辑
                    var msg = AsyncTool.DeSerialize(pack.bodyBuff);
                    OnReceiveMsg(msg);
                    pack.ResetData();
                    socket.BeginReceive(
                        pack.headBuff,
                        0,
                        AsyncPackage.headLen,
                        SocketFlags.None,
                        receiveHeadCallback,
                        pack
                        );
                }
            }
            catch (Exception e)
            {
                AsyncTool.Log("ReceiveBodyData:" + e.Message);
                CloseSession();
            }

        }

        public bool SendMsg(Pkg msg)
        {
            byte[] data = AsyncTool.Package(AsyncTool.Serialize(msg));
            return SendMsg(data);
        }

        public bool SendMsg(byte[] data)
        {
            bool result = false;

            if (sessionSate != AsyncSessionState.CONNECTED)
            {
                AsyncTool.Log("Connection is Disconnected.can not send net msg.");
            }
            else
            {
                NetworkStream ns = null;
                try
                {
                    ns = new NetworkStream(socket);
                    if (ns.CanWrite)
                    {
                        ns.BeginWrite(
                            data,
                            0,
                            data.Length,
                            sendCallback,
                            ns
                            );
                    }
                    result = true;
                }
                catch (Exception e)
                {
                    AsyncTool.Log("SendMsgNSError:{0}.", e.Message);
                }
            }
            return result;
        }

        private void SendCB(IAsyncResult ar)
        {
            NetworkStream ns = (NetworkStream)ar.AsyncState;
            try
            {
                ns.EndWrite(ar);
                ns.Flush();
                ns.Close();
            }
            catch (Exception e)
            {
                AsyncTool.Log("SendMsgNSError:{0}", e.Message);
            }
        }

        protected abstract void OnReceiveMsg(Pkg msg);

        protected abstract void OnConnected(bool result);


        protected abstract void OnDisconnected();


        public void CloseSession()
        {
            sessionSate = AsyncSessionState.NONCONNECTED;
            OnDisconnected();

            closeCallback?.Invoke();
            try
            {
                if (socket != null)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    socket = null;
                }
            }
            catch (Exception e)
            {
                AsyncTool.Log("ShutDown error :{0}", e.Message);
            }

        }
    }
}
