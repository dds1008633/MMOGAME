using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public sealed partial class EventPool<T> where T : BaseEventArgs
{
    private Dictionary<int, EventHandler<T>> eventHandlers;
    private Queue<Event> events;
    private Dictionary<object, LinkedListNode<EventHandler<T>>> cacheNodes;
    private Dictionary<object, LinkedListNode<EventHandler<T>>> tempNodes;
    private EventPoolMode eventPoolMode;
    private EventHandler<T> defalutHandler;
    /// <summary>
    /// 事件池初始化
    /// </summary>
    /// <param name="mode">事件池模式</param>
    public EventPool(EventPoolMode mode)
    {
        eventHandlers = new Dictionary<int, EventHandler<T>>();
        events = new Queue<Event>();
        cacheNodes = new Dictionary<object, LinkedListNode<EventHandler<T>>>();
        tempNodes = new Dictionary<object, LinkedListNode<EventHandler<T>>>();
        eventPoolMode = mode;
        defalutHandler = null;
    }

}

