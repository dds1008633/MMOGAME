using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

/// <summary>
/// 引用池
/// </summary>
public static partial class ReferencePool
{
    private static Dictionary<Type,ReferenceCollection> referenceCollections= new Dictionary<Type,ReferenceCollection>();
    public static bool EnableStrictCheck=false; //是否开启强制检查


    /// <summary>
    /// 获取引用池的数量
    /// </summary>
    public static int Count
    {
        get
        {
            return referenceCollections.Count;
        }
    }

    /// <summary>
    /// 获取所有引用池信息
    /// </summary>
    /// <returns></returns>
    public static ReferencePoolInfo[] GetAllReferencePoolInfos()
    {
        int index = 0;
        ReferencePoolInfo[] results = null;
        lock (referenceCollections)
        {
            results=new ReferencePoolInfo[referenceCollections.Count];
            foreach(KeyValuePair<Type,ReferenceCollection> referenceCollection in referenceCollections)
            {
                results[index++]=new ReferencePoolInfo(referenceCollection.Key, referenceCollection.Value.references.Count,referenceCollection.Value.UsingReferenceCount,referenceCollection.Value.AcquireReferenceCount,
                    referenceCollection.Value.ReleaseReferenceCount,referenceCollection.Value.AddReferenceCount,referenceCollection.Value.RemoveReferenceCount);
            }
            return results;
        }
    }
    /// <summary>
    /// 清除所有引用池
    /// </summary>
    public static void ClearAll()
    {
        lock(referenceCollections)
        {
            foreach (KeyValuePair<Type,ReferenceCollection>referenceCollection in referenceCollections)
            {
                referenceCollection.Value.RemoveAll();
            }
            referenceCollections.Clear();
        }
    }

    private static ReferenceCollection GetReferenceCollection(Type referenceType)
    {
        if (referenceType == null)
        {
            LogTool.Log("Type is invalid", Tools.ConsoleColor.Red);
            return null;
        }
        ReferenceCollection referenceCollection = null;
        lock (referenceCollections)
        {
            if(!referenceCollections.TryGetValue(referenceType, out referenceCollection))
            {
                referenceCollection=new ReferenceCollection(referenceType);
                referenceCollections.Add(referenceType, referenceCollection);
            }
        }
        return referenceCollection;
    }

    private static void CheckReferenceType(Type referenceType)
    {
        if (!EnableStrictCheck) return;

        if(referenceType == null)
        {
            LogTool.Log("referenceType is invalid", Tools.ConsoleColor.Red); 
            return;
        }

        if(!referenceType.IsClass||referenceType.IsAbstract)
        {
            LogTool.Log("referenceType is not a non-abstract class type.", Tools.ConsoleColor.Red);
            return;
        }

        if (!typeof(IReference).IsAssignableFrom(referenceType))
        {
            LogTool.Log($"referenceType :{referenceType.FullName} is invalid.", Tools.ConsoleColor.Red);
            return;
        }


    }

    /// <summary>
    /// 从引用池获取引用
    /// </summary>
    /// <typeparam name="T">引用类型</typeparam>
    /// <returns></returns>
    public static T Acquire<T>() where T : class, IReference, new()
    {
        return GetReferenceCollection(typeof(T)).Acquire<T>();
    }

    /// <summary>
    /// 从引用池获取引用。
    /// </summary>
    /// <param name="referenceType">引用类型。</param>
    /// <returns>引用。</returns>
    public static IReference Acquire(Type referenceType)
    {
        CheckReferenceType(referenceType);
        return GetReferenceCollection(referenceType).Acquire();
    }

    /// <summary>
    /// 回池
    /// </summary>
    /// <param name="reference">引用</param>
    public static void Release(IReference reference)
    {
        if (reference == null)
        {
            LogTool.Log("reference is invalid", Tools.ConsoleColor.Red);
            return;
        }
        Type referenceType=reference.GetType();
        CheckReferenceType(referenceType);
        GetReferenceCollection(referenceType).Release(reference);
    }

    /// <summary>
    /// 向引用池中追加指定数量的引用。
    /// </summary>
    /// <typeparam name="T">引用类型。</typeparam>
    /// <param name="count">追加数量。</param>
    public static void Add<T>(int count) where T : class, IReference, new()
    {
        GetReferenceCollection(typeof(T)).Add<T>(count);
    }

    /// <summary>
    /// 向引用池中追加指定数量的引用。
    /// </summary>
    /// <param name="referenceType">引用类型。</param>
    /// <param name="count">追加数量。</param>
    public static void Add(Type referenceType, int count)
    {
        CheckReferenceType(referenceType);
        GetReferenceCollection(referenceType).Add(count);
    }

    /// <summary>
    /// 从引用池中移除指定数量的引用。
    /// </summary>
    /// <typeparam name="T">引用类型。</typeparam>
    /// <param name="count">移除数量。</param>
    public static void Remove<T>(int count) where T : class, IReference
    {
        GetReferenceCollection(typeof(T)).Remove(count);
    }

    /// <summary>
    /// 从引用池中移除指定数量的引用。
    /// </summary>
    /// <param name="referenceType">引用类型。</param>
    /// <param name="count">移除数量。</param>
    public static void Remove(Type referenceType, int count)
    {
        CheckReferenceType(referenceType);
        GetReferenceCollection(referenceType).Remove(count);
    }

    /// <summary>
    /// 从引用池中移除所有的引用。
    /// </summary>
    /// <typeparam name="T">引用类型。</typeparam>
    public static void RemoveAll<T>() where T : class, IReference
    {
        GetReferenceCollection(typeof(T)).RemoveAll();
    }

    /// <summary>
    /// 从引用池中移除所有的引用。
    /// </summary>
    /// <param name="referenceType">引用类型。</param>
    public static void RemoveAll(Type referenceType)
    {
        CheckReferenceType(referenceType);
        GetReferenceCollection(referenceType).RemoveAll();
    }

}

