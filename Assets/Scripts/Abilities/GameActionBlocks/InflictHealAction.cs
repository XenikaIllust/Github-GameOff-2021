using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Definitions/Game Action/Inflict Heal")]
public class InflictHealAction : GameActionBlock
{
    public override void Invoke(float abilityStat, List<object> currentFilteredTargets,
        Dictionary<string, object> otherTargets)
    {
        foreach (object target in currentFilteredTargets)
        {
            Unit targetUnit = (Unit)target;
            float healStat = abilityStat;
            targetUnit.unitEventHandler.RaiseEvent("OnHealTaken", healStat);
        }
    }
}