using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// -------------------
///   Event Processor
/// -------------------
/// 
/// A generic system for processing game messages.
/// Useful for sending messages between components on the same object, which don't need to
/// be broadcast out to other objects.
/// </summary>
public class EventProcessor
{
    private Dictionary<string, UnityEvent<object>> eventDictionary;

    public EventProcessor()
    {
        eventDictionary = new Dictionary<string, UnityEvent<object>>();
    }

    /// <summary>
    /// Adds the passed in listener method to the event dictionary.
    /// </summary>
    /// <param name="eventName">The event being referenced.</param>
    /// <param name="listener">The listener method you would like to subscribe.</param>
    public void StartListening(string eventName, UnityAction<object> listener)
    {
        UnityEvent<object> newEvent = null;

        if (eventDictionary.TryGetValue(eventName, out newEvent))
        {
            newEvent.AddListener(listener);
        }
        else
        {
            newEvent = new UnityEvent<object>();
            newEvent.AddListener(listener);

            eventDictionary.Add(eventName, newEvent);
        }
    }

    /// <summary>
    /// Removes the passed in listener from the specified event.
    /// </summary>
    /// <param name="eventName">The event being referenced.</param>
    /// <param name="listener">The listener method you would like to unsubscribe.</param>
    public void StopListening(string eventName, UnityAction<object> listener)
    {
        UnityEvent<object> newEvent = null;

        if (eventDictionary.TryGetValue(eventName, out newEvent))
        {
            newEvent.RemoveListener(listener);
        }
    }

    /// <summary>
    /// Calls all listener functions for the specified event.
    /// </summary>
    /// <param name="eventName">The event you are trying to raise.</param>
    /// <param name="param">Additional data to pass along to the callbacks. If not applicable, pass null.</param>
    public void RaiseEvent(string eventName, object param)
    {
        UnityEvent<object> thisEvent = null;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(param);
        }
    }
}
