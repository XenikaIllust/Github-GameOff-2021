using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// -----------------
///   Event Manager
/// -----------------
/// 
/// Singleton messaging system for game events.
/// 
/// WHEN TO USE: This class should be used when objects need to notify other objects in the scene of something happening.
/// </summary>
public class EventManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent<object>> eventDictionary;

    private static EventManager eventManager;
    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType<EventManager>();

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Initialize();
                }
            }

            return eventManager;
        }
    }

    private void Initialize()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent<object>>();
        }
    }

    /// <summary>
    /// Adds an event to the eventDictionary so that we can raise it later.
    /// If this event already exists, then the listener method will be added to the
    /// UnityEvent as a listener.
    /// </summary>
    /// <param name="eventName">The event you are referring to (case sensitive).</param>
    /// <param name="listener">The listener method. Will get called when this event is raised.</param>
    public static void StartListening(string eventName, UnityAction<object> listener)
    {
        UnityEvent<object> newEvent = null;

        if (instance.eventDictionary.TryGetValue(eventName, out newEvent))
        {
            newEvent.AddListener(listener);
        }
        else
        {
            newEvent = new UnityEvent<object>();
            newEvent.AddListener(listener);

            instance.eventDictionary.Add(eventName, newEvent);
        }
    }

    /// <summary>
    /// Removes a listener from the eventDictionary. If no match is found, will not cause an error.
    /// </summary>
    /// <param name="eventName">The event being unsubscribed from.</param>
    /// <param name="listener">The listener method that is unsubscribing.</param>
    public static void StopListening(string eventName, UnityAction<object> listener)
    {
        if (eventManager == null)
            return;

        UnityEvent<object> newEvent = null;

        if (instance.eventDictionary.TryGetValue(eventName, out newEvent))
        {
            newEvent.RemoveListener(listener);
        }
    }

    /// <summary>
    /// Calls all listeners of the specified event.
    /// </summary>
    /// <param name="eventName">The event being raised.</param>
    /// <param name="param">Data to pass along with this message. If not neccesary, use null.</param>
    public static void RaiseEvent(string eventName, object param)
    {
        UnityEvent<object> thisEvent = null;

        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(param);
        }
    }
}
