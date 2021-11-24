using UnityEngine;

public class Agent : MonoBehaviour
{
    protected EventProcessor unitEventHandler;
    protected Unit thisUnit;

    protected virtual void Awake()
    {
        unitEventHandler = GetComponent<UnitEventManager>().UnitEventHandler;
        thisUnit = GetComponent<Unit>();
    }
}