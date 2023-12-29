using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 事件基类
/// </summary>
public class BaseEventArgs : EventArgs, IReference
{

    protected int id;
    /// <summary>
    /// 获取类型编号
    /// </summary>
    public int Id
    {
        get => id;
        set => id = value;
    }

    public void Clear()
    {

    }

}

public class TestEventArgs : BaseEventArgs
{

    public string aaaa;

    public static TestEventArgs Create(int id,string aaa)
    {
        TestEventArgs args = ReferencePool.Acquire<TestEventArgs>();
        args.aaaa = aaa;
        args.Id = id;
        return args;
    }
}
