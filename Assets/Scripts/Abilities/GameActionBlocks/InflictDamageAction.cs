using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InflictDamageAction : GameActionBlock
{
    public override void Invoke(SerializableDictionary<string, float> abilityStats, List<object> currentFilteredTargets, Dictionary<string, object> otherTargets)
    {
        //hardcoded damage, change it later
        foreach(object target in currentFilteredTargets) {
            Unit targetUnit = (Unit) target;
            Debug.Log(targetUnit.gameObject + " damaged for" + "10" + "damage!");
        }
    }
}
