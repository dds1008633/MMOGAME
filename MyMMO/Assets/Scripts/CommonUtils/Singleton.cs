using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
    {
        protected static bool applicationQuitting;
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                    LogTool.LogFormat("SingletonMono<{0}> has not been created", typeof(T).Name);
                return instance;
            }
        }
        public static T CreateInstance()
        {
            if (applicationQuitting)
            {
                LogTool.LogError("Application is quitting!");
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
                            LogTool.LogErrorFormat("SingletonMono<{0}> failed initializing", typeof(T).Name);
                            return null;
                        }
                    }
                }
                else
                {
                    LogTool.LogErrorFormat("SingletonMono<{0}> has already been created", typeof(T).Name);
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

    public class Singleton<T> : IUpdater where T : Singleton<T>, new()
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
                        LogTool.LogErrorFormat("Singleton<{0}> failed initializing", typeof(T).Name);
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
        public void Remove()
        {
            onRemove(); 
            instance = null;
        }

        public void UpdaterInit()
        {
            GameUpdater.Instance.AddUpdater(this);
        }

        public virtual void Update() { }        

        public void UpdaterDestory()
        {
            GameUpdater.Instance.RemoveUpdater(this);
        }       

        protected virtual void onInit() { }
        protected virtual void onRemove() { }
       
    }
}
