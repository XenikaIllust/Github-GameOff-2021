using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class WithinAbilityRange : Conditional
{
    float abilityRange = 3; // hardcoded for testing
    Unit playerUnit;

    public override void OnAwake()
    {
        // cache the player
        playerUnit = GameObject.FindObjectOfType<PlayerAgent>().GetComponent<Unit>();
    }

    public override TaskStatus OnUpdate()
    {
        if(withinAbilityRange(playerUnit.transform)) {
            return TaskStatus.Success;
        }
        else {
            return TaskStatus.Failure;
        }
    }

    private bool withinAbilityRange(Transform targetTransform) {
        return (targetTransform.position - transform.position).magnitude <= abilityRange;
    }
}
