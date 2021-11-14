using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InflictDamageAction : GameAction
{
    float damage = 10;
    public override void Invoke(Dictionary<string, object> inputTargets, List<object> targets)
    {
        //hardcoded damage, change it later
        foreach(object target in targets) {
            Unit targetUnit = (Unit) target;
            Debug.Log(targetUnit.gameObject + " damaged for 10 damage!");
        }
    }
}
