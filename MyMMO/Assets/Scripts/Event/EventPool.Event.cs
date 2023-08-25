using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public sealed partial class EventPool<T> where T : BaseEventArgs
{
    private sealed class Event : IReference
    {
        public object Sender;
        public T EventArgs;

        public static Event Create(object sender, T e)
        {
            Event eventNode = ReferencePool.Acquire<Event>();
            eventNode.Sender = sender;
            eventNode.EventArgs = e;
            return eventNode;
        }


        public void Clear()
        {
            Sender = null;
            EventArgs = null;
        }
    }



}

