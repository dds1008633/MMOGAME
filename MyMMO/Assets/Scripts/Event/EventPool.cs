using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using UnityEngine.XR;

public sealed partial class EventPool<T> where T : BaseEventArgs
{
    private Dictionary<int, EventHandler<T>> eventHandlers;
    private Queue<Event> events;   
    /// <summary>
    /// 事件池初始化
    /// </summary>
    /// <param name="mode">事件池模式</param>
    public EventPool(EventPoolMode mode)
    {
        eventHandlers = new Dictionary<int, EventHandler<T>>();
        events = new Queue<Event>();
       
    }
    /// <summary>
    /// 事件池轮询
    /// </summary>
    public void Update()
    {
        lock (events)
        {
            while (events.Count > 0)
            {

                Event eventNode = events.Dequeue();
                HandleEvent(eventNode.Sender, eventNode.EventArgs);
                ReferencePool.Release(eventNode);
            }
        }
    }

    /// <summary>
    /// 处理事件节点
    /// </summary>
    /// <param name="sender">事件源</param>
    /// <param name="e">事件参数</param>
    private void HandleEvent(object sender, T e)
    {
        if(eventHandlers.TryGetValue(e.Id,out var handler))
        {
            handler(sender, e);
            ReferencePool.Release(e);
        }
    }

    /// <summary>
    /// 检查是否存在事件处理函数
    /// </summary>
    /// <param name="id">事件类型编号</param>   
    /// <returns></returns>
    public bool Check(int id)
    {
        return eventHandlers.ContainsKey(id);
    }

    /// <summary>
    /// 订阅事件处理函数
    /// </summary>
    /// <param name="id">事件类型编号</param>
    /// <param name="handler">要订阅的事件处理函数</param>
    public void Subscribe(int id, EventHandler<T> handler)
    {
        if (!eventHandlers.ContainsKey(id))
        {
            eventHandlers.Add(id, handler);
        }
    }

    /// <summary>
    /// 取消订阅事件处理函数
    /// </summary>
    /// <param name="id"></param>
    public void Unsubscribe(int id)
    {
        if (!eventHandlers.Remove(id))
        {
            LogTool.LogErrorFormat("Event '{0}' not exists specified handler.", id);
        }
    }

    /// <summary>
    /// 抛出事件，事件会在抛出后的下一帧分发。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Fire(object sender,T e)
    {
        Event eventNode=Event.Create(sender, e);
        lock (events)
        {
            events.Enqueue(eventNode);
        }
    }

    /// <summary>
    /// 抛出事件立即模式，事件会立刻分发。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void FireNow(object sender, T e)
    {
        HandleEvent(sender, e);
    }
}

