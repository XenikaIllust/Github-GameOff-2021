using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/Game Action/SilenceRandomAbility")]
public class SilenceRandomAbilityActionBlock : GameActionBlock
{
    public override void Invoke(float abilityStat, List<object> currentFilteredTargets,
        Dictionary<string, object> otherTargets)
    {
        Debug.Log("Silence executed!");
        foreach (object target in currentFilteredTargets)
        {
            Unit targetUnit = (Unit)target;
            float silenceDurationStat = abilityStat;
            int targetAbilityIndex = UnityEngine.Random.Range(1, 5);
            Tuple<int, float> parameter = new Tuple<int, float>(targetAbilityIndex, silenceDurationStat);
            targetUnit.unitEventHandler.RaiseEvent("OnAbilityLocked", parameter);
            Debug.Log(targetAbilityIndex + " locked!");
        }
    }
}
