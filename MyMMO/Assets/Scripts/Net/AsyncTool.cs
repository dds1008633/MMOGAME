using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using LogicProtocol;
namespace Net
{
    public class AsyncTool
    {
        public static byte[] Package(byte[] data)
        {
            int len = data.Length;
            byte[] pack = new byte[len + 4];
            byte[] head = BitConverter.GetBytes(len);
            head.CopyTo(pack, 0);
            data.CopyTo(pack, 4);
            return pack;
        }

        public static byte[] Serialize(Pkg msg)
        {
            byte[] data = null;
            MemoryStream ms = new MemoryStream();
            try
            {
                Serializer.Serialize(ms, msg);
                ms.Seek(0, SeekOrigin.Begin);
                data = ms.ToArray();
            }
            catch (SerializationException e)
            {
                Log("Faild to serialize.Reson:{0}", e.Message);
            }
            finally
            {
                ms.Close();
            }
            return data;
        }

        public static Pkg DeSerialize(byte[] bytes)
        {
            Pkg msg = null;
            MemoryStream ms = new MemoryStream(bytes);
            try
            {
                msg = Serializer.Deserialize<Pkg>(ms);
            }
            catch (SerializationException e)
            {
                Log("Faild to deserialize.Reson:{0} bytesLen:{1}.", e.Message, bytes.Length);
            }
            finally
            {
                ms.Close();
            }

            return msg;
        }

        public static Action<string> LogFunc;

        public static void Log(string msg, params object[] args)
        {
            msg = string.Format(msg, args);
            if (LogFunc != null)
            {
                LogFunc(msg);
            }
            else
            {
                UnityEngine.Debug.Log(msg);
            }
        }
    }
}
