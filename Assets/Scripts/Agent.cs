using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour
{
    protected EventProcessor unitEventHandler;

    protected virtual void Awake() {
        unitEventHandler = GetComponent<UnitEventManager>().UnitEventHandler; 
    }

    public virtual IEnumerator ProcessTargetInput(Ability ability) {
        yield return null;
    }
}