using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class AbilityInUse : Conditional
{
    public SharedBool abilityInUse;
    
    public override TaskStatus OnUpdate() {
        if(abilityInUse.Value) {
            return TaskStatus.Failure;
        }
        else {
            return TaskStatus.Success;
        }
    }
}
