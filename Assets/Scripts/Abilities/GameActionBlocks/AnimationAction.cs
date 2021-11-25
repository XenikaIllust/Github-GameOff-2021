using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/Game Action/Animation")]
public class AnimationAction : GameActionBlock
{
    public override void Invoke(string[] idParams, List<object> currentFilteredTargets, Dictionary<string, object> otherTargets)
    {
        Unit unitToAnimate = (Unit) currentFilteredTargets[0];

        unitToAnimate.unitEventHandler.RaiseEvent("OnChangeAnimation", idParams[0]);
    }
}
