using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DisappearActionData {
    public Unit unitToDisappear;

    public DisappearActionData(Unit u) {
        unitToDisappear = u;
    }
}

[CreateAssetMenu(menuName = "Definitions/Game Action/Disappear")]
public class DisappearAction : GameAction
{
    public override void Invoke(object param) {
        DisappearActionData disappearActionData = (DisappearActionData) param;

        // become invisible
        disappearActionData.unitToDisappear.GetComponent<SpriteRenderer>().enabled = false;

        // allow phasing through enemy units
    }
}
