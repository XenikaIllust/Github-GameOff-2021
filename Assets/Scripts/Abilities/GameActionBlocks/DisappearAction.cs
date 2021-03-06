using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/Game Action/Disappear")]
public class DisappearAction : GameActionBlock
{
    public override void Invoke(List<object> currentFilteredTargets, Dictionary<string, object> otherTargets) 
    {
        Unit unitToDisappear = (Unit) currentFilteredTargets[0];

        // become invisible
        unitToDisappear.GetComponentInChildren<SpriteRenderer>().enabled = false;

        // allow phasing through enemy units
    }
}
