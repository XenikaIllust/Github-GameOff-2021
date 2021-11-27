using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/Game Action/Aim")]
public class AimAction : GameActionBlock
{
    public override void Invoke(float abilityStat, List<object> currentFilteredTargets,
        Dictionary<string, object> otherTargets)
    {
        Unit aimingUnit = (Unit)currentFilteredTargets[0];
        aimingUnit.unitEventHandler.RaiseEvent("OnLookForced", abilityStat);
    }
}