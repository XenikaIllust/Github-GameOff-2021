using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Definitions/Game Action/AimActionBlock")]
public class AimActionBlock : GameActionBlock
{
    public override void Invoke(float abilityStat, List<object> currentFilteredTargets, Dictionary<string, object> otherTargets)
    {
        Unit aimingUnit = (Unit) currentFilteredTargets[0];

        aimingUnit.movementSpeed = 0f;
        aimingUnit.turnRate = abilityStat;
    }
}
