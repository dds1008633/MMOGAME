using OpenCover.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

public static partial class ReferencePool
{
    private sealed class ReferenceCollection
    {
        public Queue<IReference> references;
        private Type referenceType;

        public int UsingReferenceCount;
        public int AcquireReferenceCount;
        public int AddReferenceCount;
        public int ReleaseReferenceCount;
        public int RemoveReferenceCount;
        public ReferenceCollection(Type referenceType)
        {
            references = new Queue<IReference>();
            this.referenceType = referenceType;
        }

        public T Acquire<T>() where T : class, IReference, new()
        {
            if (typeof(T) != referenceType)
            {
                LogTool.Log("Type is invalid", Tools.ConsoleColor.Red);
                return null;
            }
            UsingReferenceCount++;
            AcquireReferenceCount++;
            lock (references)
            {
                if (references.Count > 0)
                {
                    return (T)references.Dequeue();
                }
            }
            AddReferenceCount++;
            return new T();
        }

        public IReference Acquire()
        {
            UsingReferenceCount++;
            AcquireReferenceCount++;
            lock (references)
            {
                if (references.Count > 0)
                {
                    return references.Dequeue();
                }
            }
            AddReferenceCount++;
            return (IReference)Activator.CreateInstance(referenceType);
        }

        public void Release(IReference reference)
        {
            reference.Clear();
            lock (references)
            {
                if (EnableStrictCheck && references.Contains(reference))
                {
                    LogTool.Log("The reference has been released", Tools.ConsoleColor.Red);
                    return;
                }
                references.Enqueue(reference);
            }
            ReleaseReferenceCount++;
            UsingReferenceCount--;
        }

        public void Add<T>(int count) where T : class, IReference, new()
        {
            if (typeof(T) != referenceType)
            {
                LogTool.Log("Type is invalid", Tools.ConsoleColor.Red);                
            }

            lock (references)
            {
                AddReferenceCount+= count;
                while (count-- > 0)
                {
                    references.Enqueue(new T());
                }
            }
        }

        public void Add(int count)
        {
            lock (references)
            {
                AddReferenceCount += count;
                while (count-- > 0)
                {
                    references.Enqueue((IReference)Activator.CreateInstance(referenceType));
                }
            }
        }

        public void Remove(int count)
        {
            lock(references)
            {
                count = count>references.Count? count:count;
                RemoveReferenceCount += count;
                while(count-- > 0)
                {
                    references.Dequeue();
                }
            }
        }

        public void RemoveAll()
        {
            lock (references)
            {
                RemoveReferenceCount += references.Count;
                references.Clear();
            }
        }
    }
}

