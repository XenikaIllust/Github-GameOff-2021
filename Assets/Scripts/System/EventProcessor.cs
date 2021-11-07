using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventProcessor
{
    private Dictionary<string, UnityEvent<object>> eventDictionary;

    private EventProcessor eventProcessor;
    public EventProcessor instance => eventProcessor;

    public EventProcessor()
    {
        eventDictionary = new Dictionary<string, UnityEvent<object>>();
    }

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

    public void StopListening(string eventName, UnityAction<object> listener)
    {
        if (eventProcessor == null)
            return;

        UnityEvent<object> newEvent = null;

        if (eventDictionary.TryGetValue(eventName, out newEvent))
        {
            newEvent.RemoveListener(listener);
        }
    }

    public void RaiseEvent(string eventName, object param)
    {
        UnityEvent<object> thisEvent = null;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(param);
        }
    }
}
