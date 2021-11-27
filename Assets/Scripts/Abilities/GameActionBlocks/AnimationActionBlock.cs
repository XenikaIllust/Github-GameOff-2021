using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/Game Action/AnimationBlock")]
public class AnimationActionBlock : GameActionBlock
{
    public override void Invoke(string[] idParams, List<object> currentFilteredTargets, Dictionary<string, object> otherTargets)
    {
        /*---------------------------------------------------------------------------------------
        AnimationAction is meant to only play an animation on the unit executing the ability.
        idParams[0] contains the name of the animation trigger to trigger.
        ---------------------------------------------------------------------------------------*/
        Unit self = (Unit) otherTargets["Executing Unit"];
        UnitAnimationManager animationManager = self.GetComponentInChildren<UnitAnimationManager>();
        animationManager.Animator.SetBool(idParams[0], true);
    }
}
