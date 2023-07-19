using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
{
    protected static bool applicationQuitting;
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
                Debug.LogFormat("SingletonMono<{0}> has not been created", typeof(T).Name);
            return instance;
        }
    }
    public static T CreateInstance()
    {
        if (applicationQuitting)
        {
            Debug.LogError("Application is quitting!");
            return null;
        }

        if (instance == null)
        {
            instance = GameObject.FindObjectOfType<T>();
            if (instance == null)
            {
                instance = new GameObject().AddComponent<T>();
                {
                    if (instance == null)
                    {
                        Debug.LogErrorFormat("SingletonMono<{0}> failed initializing", typeof(T).Name);
                        return null;
                    }
                }
            }
            else
            {
                Debug.LogErrorFormat("SingletonMono<{0}> has already been created", typeof(T).Name);
            }
            instance.onInit(); 
        }
        return instance;
    }
    
    protected virtual void onApplicationQuit() { }    
    protected virtual void onDestroy() { }
    protected virtual void onInit() { }

    protected bool callDestroySelf = false;
    public void DestroySelf(bool immediately = false)
    {
        callDestroySelf = true;
        onDestroy();
        if (immediately)
            DestroyImmediate(gameObject);
        else
            Destroy(gameObject);
        instance = null;
    }
}

public class Singleton<T> where T : Singleton<T>, new()
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
                if (instance == null)
                {
                    Debug.LogErrorFormat("Singleton<{0}> failed initializing", typeof(T).Name);
                    return null;
                }
                instance.onInit();
            }
            return instance;
        }
    }
    public static T CreateInstance()
    {
        return Instance;
    }
    public void Remove() { onRemove(); instance = null; }
    protected virtual void onInit() { }
    protected virtual void onRemove() { }
}