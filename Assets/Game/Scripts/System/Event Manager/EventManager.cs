using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Event manager for app- and game-specific events.
/// Add and remove listeners here.
/// Singleton interface to make sure there is only one of this in app.
/// </summary>
public class EventManager : Singleton<EventManager>
{
    /// <summary>
    /// Base class for all events. Used by NEventManager.
    /// </summary>
    public class Event
    {
        // Specifies whether this event is in use. Guards against using it more than once at a time.
        public bool InUse { get; set; }

        /// <summary>
        /// Reset any state, including in use flag.
        /// </summary>
        public virtual void Reset()
        {
            InUse = false;
        }
    }

    // Specify delegate type for events
    public delegate void EventDelegate<T>(T e) where T : Event;
    private delegate void EventDelegate(Event e);

    //public delegate void TaskCompletedDelegate();

    // Map to store generic delegates.
    private Dictionary<System.Type, EventDelegate> _delegates = new Dictionary<System.Type, EventDelegate>();

    // Map to store non-generic delegates that point to generic delegates.
    private Dictionary<System.Delegate, EventDelegate> _delegateLookup = new Dictionary<System.Delegate, EventDelegate>();

    private Dictionary<System.Type, List<Event>> _events = new Dictionary<System.Type, List<Event>>();

    private void checkin() { }

    void Awake()
    {
        Instance.checkin();
    }

    /// <summary>
    /// Add new listener for this event delegate
    /// </summary>
    /// <typeparam name="T">Type of event listener (delegate)</typeparam>
    /// <param name="del">Listener (delegate) to add</param>
    public void AddListener<T>(EventDelegate<T> del) where T : Event
    {
        // Early out of already added
        if (_delegateLookup.ContainsKey(del))
        {
            return;
        }

        // Create new non-generic delegate which calls our generic one
        // This is the delegate we invoke
        EventDelegate internalDelegate = (e) => del((T)e);
        _delegateLookup[del] = internalDelegate;

        // If already has a delegate for this type, add to it. Otherwise set it.
        EventDelegate tempDel;
        if (_delegates.TryGetValue(typeof(T), out tempDel))
        {
            _delegates[typeof(T)] = tempDel += internalDelegate;
        }
        else
        {
            _delegates[typeof(T)] = internalDelegate;
        }
    }



    /// <summary>
    /// Remove listener from list.
    /// </summary>
    /// <typeparam name="T">Type of event listener (delegate)</typeparam>
    /// <param name="del">Listener (deleage) to remove</param>
    public void RemoveListener<T>(EventDelegate<T> del) where T : Event
    {
        // First look up the delegate to get our internal mapping
        EventDelegate internalDelegate;
        if (_delegateLookup.TryGetValue(del, out internalDelegate))
        {
            // Now look up to get internal delegate
            EventDelegate tempDel;
            if (_delegates.TryGetValue(typeof(T), out tempDel))
            {
                // Remove from internal delegate list, and delete that list if last element
                tempDel -= internalDelegate;
                if (tempDel == null)
                {
                    _delegates.Remove(typeof(T));
                }
                else
                {
                    _delegates[typeof(T)] = tempDel;
                }
            }

            // Remove from internal mapping
            _delegateLookup.Remove(del);
        }
    }



    /// <summary>
    /// Raise the event so that all listeners get a callback.
    /// </summary>
    /// <param name="e">The event to raise</param>
    public void Raise(Event e)
    {
        // Automatically set to be in use. This avoids re-use while the event is being invoked.
        e.InUse = true;

        // Find and invoke the delegate
        EventDelegate del;
        if (_delegates.TryGetValue(e.GetType(), out del))
        {
            del.Invoke(e);
        }

        // Reset its state after use
        e.Reset();
    }

    /// <summary>
    /// Look up the event, if one is not there or free then we create and use that one
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Event GetEvent<T>()
    {
        System.Type type = typeof(T);

        // Find event type from map, otherwise create it if it doesn't exist
        if (!_events.ContainsKey(type))
        {
            // Add new event type list
            _events.Add(type, new List<Event>());
        }
        else
        {
            // Find in list
            int numEvents = _events[type].Count;
            for (int i = 0; i < numEvents; ++i)
            {
                if (!_events[type][i].InUse)
                {
                    return _events[type][i];
                }
            }
        }

        // Add new event
        Event newEvent = System.Activator.CreateInstance(type) as Event;
        if (newEvent != null)
        {
            _events[type].Add(newEvent);
        }
        else
        {
            Debug.LogErrorFormat("Event {0} cannot be created", type.ToString());
        }
        return newEvent;
    }

    public void OnDisable()
    {
        // Clear all events
        _events.Clear();
    }
}