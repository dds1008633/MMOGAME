using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �¼�����
/// </summary>
public abstract class BaseEventArgs : EventArgs, IReference
{

    /// <summary>
    /// ��ȡ���ͱ��
    /// </summary>
    public abstract int Id
    {
        get;
    }

    public abstract void Clear();
    
}
