using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class AbilityOnCooldown : Conditional
{
    public SharedBool cooldownActive;
    
    public override TaskStatus OnUpdate() {
        if(cooldownActive.Value) {
            return TaskStatus.Failure;
        }
        else {
            return TaskStatus.Success;
        }
    }
}
