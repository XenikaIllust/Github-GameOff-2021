using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/Game Action/Reappear")]
public class ReappearAction : GameActionBlock
{
    public override void Invoke(SerializableDictionary<string, float> abilityStats, List<object> currentFilteredTargets, Dictionary<string, object> otherTargets)
    {
        Unit unitToReappear = (Unit) currentFilteredTargets[0];

        // become visible
        unitToReappear.GetComponentInChildren<SpriteRenderer>().enabled = true;

        // disallow phasing through enemy units
    }
}   
