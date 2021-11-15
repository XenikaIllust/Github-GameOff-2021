using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/Game Action/Disappear")]
public class DisappearAction : GameActionBlock
{
    public override void Invoke(Dictionary<string, float> abilityStats, List<object> currentFilteredTargets, Dictionary<string, object> otherTargets) 
    {
        Unit unitToDisappear = (Unit) currentFilteredTargets[0];
        Debug.Log(unitToDisappear.gameObject.name);

        // become invisible
        unitToDisappear.GetComponentInChildren<SpriteRenderer>().enabled = false;

        // allow phasing through enemy units
    }
}
