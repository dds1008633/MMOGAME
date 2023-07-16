using System;
using System.Collections.Generic;
using System.Text;

namespace Net
{
    public class AsyncPackage
    {
        public const int headLen = 4;
        public byte[] headBuff = null;
        public int headIndex = 0;

        public int bodyLen = 0;
        public byte[] bodyBuff = null;
        public int bodyIndex = 0;

        public AsyncPackage() 
        { 
            headBuff = new byte[headLen];
        }

        public void InitBodyBuff()
        {
            bodyLen=BitConverter.ToInt32(headBuff, 0);
            bodyBuff = new byte[bodyLen];
        }

        public void ResetData()
        {
            headIndex = 0;
            bodyIndex = 0;
            bodyBuff = null;
            bodyIndex = 0;
        }
    }
}
