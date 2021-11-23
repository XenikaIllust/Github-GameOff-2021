using UnityEngine;

public class Agent : MonoBehaviour
{
    protected EventProcessor unitEventHandler;

    protected virtual void Awake()
    {
        unitEventHandler = GetComponent<UnitEventManager>().UnitEventHandler;
    }
}