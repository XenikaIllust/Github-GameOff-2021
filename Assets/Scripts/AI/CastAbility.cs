using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CastAbility : Action
{
    public SharedBool cooldownActive;
    float coolDown = 5; // hardcoded for testing

    EventProcessor unitEventHandler;
    public override void OnAwake()
    {
        // cache the player
        unitEventHandler = GetComponent<UnitEventManager>().UnitEventHandler;
        cooldownActive.Value = false;
    }

    public override TaskStatus OnUpdate()
    {
        unitEventHandler.RaiseEvent("On1stAbilityCasted", null);
        StartCoroutine(StartCooldown(coolDown));
        return TaskStatus.Success;
    }

    private IEnumerator StartCooldown(float time) { // function for testing
        cooldownActive.Value = true;
        yield return new WaitForSeconds(time);
        cooldownActive.Value = false;
    }
}
