using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 引用池信息
/// </summary>

public struct ReferencePoolInfo
{
    public readonly Type Type;
    public readonly int UnusedReferenceCount;
    public readonly int UsingReferenceCount;
    public readonly int AcquireReferenceCount;
    public readonly int ReleaseReferenceCount;
    public readonly int AddReferenceCount;
    public readonly int RemoveReferenceCount;

    /// <summary>
    /// 初始化引用池信息新实例
    /// </summary>
    /// <param name="type"></param>
    /// <param name="unusedReferenceCount">未使用引用数量</param>
    /// <param name="usingReferenceCount">正在使用引用数量</param>
    /// <param name="acquireReferenceCount">获取引用数量</param>
    /// <param name="releaseReferenceCount">归还引用数量</param>
    /// <param name="addReferenceCount">增加引用数量</param>
    /// <param name="removeReferenceCount">移除引用数量</param>
    public ReferencePoolInfo(Type type, int unusedReferenceCount, int usingReferenceCount, int acquireReferenceCount, int releaseReferenceCount, int addReferenceCount, int removeReferenceCount)
    {
        Type = type;
        UnusedReferenceCount = unusedReferenceCount;
        UsingReferenceCount = usingReferenceCount;
        AcquireReferenceCount = acquireReferenceCount;
        ReleaseReferenceCount = releaseReferenceCount;
        AddReferenceCount = addReferenceCount;        
        RemoveReferenceCount = removeReferenceCount;        
    }

}

