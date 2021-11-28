using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent<object>> _eventDictionary;

    private static EventManager _eventManager;

    public static EventManager Instance
    {
        get
        {
            if (!_eventManager)
            {
                _eventManager = FindObjectOfType<EventManager>();

                if (!_eventManager)
                {
                    Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                }
                else
                {
                    _eventManager.Initialize();
                }
            }

            return _eventManager;
        }
    }

    private void Initialize()
    {
        _eventDictionary ??= new Dictionary<string, UnityEvent<object>>();
    }

    public static void StartListening(string eventName, UnityAction<object> listener)
    {
        if (Instance._eventDictionary.TryGetValue(eventName, out var newEvent))
        {
            newEvent.AddListener(listener);
        }
        else
        {
            newEvent = new UnityEvent<object>();
            newEvent.AddListener(listener);

            Instance._eventDictionary.Add(eventName, newEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<object> listener)
    {
        if (_eventManager == null) return;

        if (Instance._eventDictionary.TryGetValue(eventName, out var newEvent))
        {
            newEvent.RemoveListener(listener);
        }
    }

    public static void RaiseEvent(string eventName, object param)
    {
        if (Instance._eventDictionary.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent.Invoke(param);
        }
    }
}