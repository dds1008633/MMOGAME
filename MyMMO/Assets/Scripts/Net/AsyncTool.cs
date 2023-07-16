using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Net
{
    public class AsyncTool
    {
        public static byte[] PackLenInfo(byte[] data)
        {
            int len = data.Length;
            byte[] pack=new byte[len+4];
            byte[] head=BitConverter.GetBytes(len);
            head.CopyTo(pack, 0);
            data.CopyTo(pack, 4);
            return pack;
        }

        public static byte[] Serialize<T>(T msg)where T : AsyncMsg
        {
            byte[] data = null;
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                bf.Serialize(ms, msg);
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
        
        public static T DeSerialize<T>(byte[] bytes) where T : AsyncMsg
        {
            T msg = null;
            MemoryStream ms=new MemoryStream(bytes);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            try
            {
                msg=(T)binaryFormatter.Deserialize(ms);
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
