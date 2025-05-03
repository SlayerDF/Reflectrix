using System;
using System.Collections.Generic;

namespace UniTrait
{
    public class UniTraitEventBus
    {
        private readonly Dictionary<Type, Delegate> events = new();

        public void Subscribe<T>(Action<T> callback) where T : struct
        {
            if (events.TryGetValue(typeof(T), out var existing))
            {
                events[typeof(T)] = Delegate.Combine(existing, callback);
            }
            else
            {
                events[typeof(T)] = callback;
            }
        }

        public void Unsubscribe<T>(Action<T> callback) where T : struct
        {
            if (!events.TryGetValue(typeof(T), out var existing))
            {
                return;
            }

            existing = Delegate.Remove(existing, callback);
            if (existing == null)
            {
                events.Remove(typeof(T));
            }
            else
            {
                events[typeof(T)] = existing;
            }
        }

        public void Publish<T>(T @event) where T : struct
        {
            if (events.TryGetValue(typeof(T), out var callbacks))
            {
                (callbacks as Action<T>)?.Invoke(@event);
            }
        }
    }
}
