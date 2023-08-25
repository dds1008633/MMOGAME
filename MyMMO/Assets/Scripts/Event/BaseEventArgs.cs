using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 事件基类
/// </summary>
public abstract class BaseEventArgs : EventArgs, IReference
{

    /// <summary>
    /// 获取类型编号
    /// </summary>
    public abstract int Id
    {
        get;
    }

    public abstract void Clear();
    
}
