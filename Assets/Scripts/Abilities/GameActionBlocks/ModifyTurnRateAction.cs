using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Definitions/Game Action/Modify Turn Rate")]
public class ModifyTurnRateAction : GameActionBlock
{
    public override void Invoke(float abilityStat, List<object> currentFilteredTargets,
        Dictionary<string, object> otherTargets)
    {
        foreach(object target in currentFilteredTargets) {
            Unit unitToModify = (Unit) target;
            float turnRateMultiplier = abilityStat;
            unitToModify.unitEventHandler.RaiseEvent("OnTurnRateMultiplierChanged", turnRateMultiplier);
        }
    }
}
