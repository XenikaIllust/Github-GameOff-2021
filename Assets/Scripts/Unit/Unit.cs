using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// --------
///   Unit
/// --------
/// 
/// Basic framework for scene actors.
/// </summary>
public class Unit : MonoBehaviour
{
    // Event processor for handling internal messages
    private EventProcessor unitEvents;
    public EventProcessor UnitEvents => unitEvents;

    private void Awake()
    {
        // Initialize EventProcessor so we can start sending and receiving events
        unitEvents = new EventProcessor();
    }
}