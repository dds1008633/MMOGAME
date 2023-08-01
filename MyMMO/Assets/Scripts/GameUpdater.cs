using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;


public interface IUpdater
{
    void UpdaterInit();

    void Update();

    void UpdaterDestory();

}

public class GameUpdater : SingletonMono<GameUpdater>
{
    private List<IUpdater> _updates = new List<IUpdater>();
    protected override void onInit()
    {
        base.onInit();
        _updates = new List<IUpdater>();
    }

    protected override void onDestroy()
    {
        base.onDestroy();
    }

    protected override void onApplicationQuit()
    {
        base.onApplicationQuit();
    }

    private void Update()
    {
        _updates.ForEach(update => update.Update());
    }

    public void AddUpdater(IUpdater updater)
    {        
        _updates?.Add(updater);
    }

    public void RemoveUpdater(IUpdater updater)
    {
        _updates?.Remove(updater);
    }
}
