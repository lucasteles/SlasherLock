using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Systems.Observables
{
    public interface IEventSender<Event>
    {
        void SubscribeToEvent(IEventListener<Event> listener);
    }

    public abstract class EventSender<Event> : MonoBehaviour, IEventSender<Event>
    {
        protected List<IEventListener<Event>> Listeners { get; set; }
            = new List<IEventListener<Event>>();

        public void SubscribeToEvent(IEventListener<Event> listener)
           => Listeners.Add(listener);

        public void UnsubscribeFromEvent(IEventListener<Event> listener)
            => Listeners.Remove(listener);

        public void NotifyListeners(Event e)
        {
            foreach (var listener in Listeners)
                listener.Notify(e);
        }
    }
}
