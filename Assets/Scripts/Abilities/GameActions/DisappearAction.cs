using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/Game Action/Disappear")]
public class DisappearAction : GameAction
{
    public override void Invoke(Dictionary<string, object> inputTargets, List<object> targets) 
    {
        Unit unitToDisappear = (Unit) targets[0];

        // become invisible
        unitToDisappear.GetComponentInChildren<SpriteRenderer>().enabled = false;

        // allow phasing through enemy units
    }
}
