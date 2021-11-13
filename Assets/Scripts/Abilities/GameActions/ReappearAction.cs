using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/Game Action/Reappear")]
public class ReappearAction : GameAction
{
    public override void Invoke(Dictionary<string, object> inputTargets, List<object> targets)
    {
        Unit unitToReappear = (Unit) targets[0];

        // become visible
        unitToReappear.GetComponentInChildren<SpriteRenderer>().enabled = true;

        // disallow phasing through enemy units
    }
}   
