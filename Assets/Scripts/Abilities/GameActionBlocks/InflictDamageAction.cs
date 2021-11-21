using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Definitions/Game Action/Inflict Damage")]
public class InflictDamageAction : GameActionBlock
{
    public override void Invoke(float abilityStat, List<object> currentFilteredTargets,
        Dictionary<string, object> otherTargets)
    {
        foreach (object target in currentFilteredTargets)
        {
            Unit targetUnit = (Unit)target;
            float damageStat = abilityStat;
            targetUnit.unitEventHandler.RaiseEvent("OnDamageTaken", damageStat);
        }
    }
}