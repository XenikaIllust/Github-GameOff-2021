using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/Game Action/StealRandomAbility")]
public class StealRandomAbilityActionBlock : GameActionBlock
{
    public override void Invoke(List<object> currentFilteredTargets,
        Dictionary<string, object> otherTargets)
    {
        Debug.Log("Steal Random Ability executed!");
        foreach (object target in currentFilteredTargets)
        {
            Unit targetUnit = (Unit)target;

            if(targetUnit.GetType() != typeof(MainCharacterUnit)) {
                return;
            }

            int targetAbilityIndex = UnityEngine.Random.Range(1, 5);
            Tuple<int, float> parameter = new Tuple<int, float>(targetAbilityIndex, Mathf.Infinity);
            targetUnit.unitEventHandler.RaiseEvent("OnAbilityLocked", parameter);
            Debug.Log(targetAbilityIndex + " locked!");
            otherTargets["StolenAbilityIndex"] = targetAbilityIndex;
            return; 
        }
    }
}
