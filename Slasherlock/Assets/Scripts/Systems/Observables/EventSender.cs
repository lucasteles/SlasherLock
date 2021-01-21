using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Systems.Observables
{

    public abstract class EventSender<TEvent> : MonoBehaviour
    {
        protected List<IEventListener<TEvent>> Listeners { get; set; }
            = new List<IEventListener<TEvent>>();

        public void SubscribeToEvent(IEventListener<TEvent> listener)
           => Listeners.Add(listener);

        public void UnsubscribeFromEvent(IEventListener<TEvent> listener)
            => Listeners.Remove(listener);

        public void NotifyListeners(TEvent e)
        {
            foreach (var listener in Listeners)
                listener.Notify(e);
        }
    }
}
