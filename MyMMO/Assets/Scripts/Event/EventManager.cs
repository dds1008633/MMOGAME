using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using UnityEngine;

/// <summary>
/// 事件管理
/// </summary>
public class EventManager:Singleton<EventManager>
{

    private EventPool<BaseEventArgs> eventPool;

    public override void Update()
    {
        this.eventPool.Update();
    }

    protected override void onInit()
    {
        base.onInit();
        UpdaterInit();
        eventPool = new EventPool<BaseEventArgs>(EventPoolMode.Default);
    }

    protected override void onRemove()
    {
        base.onRemove();
        UpdaterDestory();
    }

    /// <summary>
    /// 订阅事件处理函数
    /// </summary>
    /// <param name="id">事件类型编号</param>
    /// <param name="handler">要订阅的事件处理函数</param>
    public void Subscribe(int id, EventHandler<BaseEventArgs> handler)
    {
        this.eventPool.Subscribe(id, handler);
    }

    /// <summary>
    /// 取消订阅事件处理函数
    /// </summary>
    /// <param name="id"></param>
    public void Unsubscribe(int id)
    {
       this.eventPool.Unsubscribe(id);
    }

    /// <summary>
    /// 抛出事件，事件会在抛出后的下一帧分发。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Fire(object sender, BaseEventArgs e)
    {
       this.eventPool.Fire(sender, e);
    }

    /// <summary>
    /// 抛出事件立即模式，事件会立刻分发。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void FireNow(object sender, BaseEventArgs e)
    {
        this.eventPool.FireNow(sender, e);
    }
}

