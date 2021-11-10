using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEventManager: MonoBehaviour
{
    EventProcessor unitEventHandler;
    public EventProcessor UnitEventHandler {
        get {return unitEventHandler;}
    }

    private void Awake() {
        unitEventHandler = new EventProcessor();
    }
}
