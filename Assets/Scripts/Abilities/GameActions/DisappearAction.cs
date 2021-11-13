using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/Game Action/Disappear")]
public class DisappearAction : GameAction
{
    public override void Invoke(List<object> targets) {
        Unit unitToDisappear = (Unit) targets[0];

        // become invisible
        unitToDisappear.GetComponent<SpriteRenderer>().enabled = false;

        // allow phasing through enemy units
    }
}
