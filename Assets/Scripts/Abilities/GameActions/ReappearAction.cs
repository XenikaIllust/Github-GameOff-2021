using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ReappearActionData {
    public Unit unitToReappear;

    public ReappearActionData(Unit u) {
        unitToReappear = u;
    }
}

[CreateAssetMenu(menuName = "Definitions/Game Action/Reappear")]
public class ReappearAction : GameAction
{
    public override void Invoke(object param)
    {
        ReappearActionData reappearActionData = (ReappearActionData) param;

        // become invisible
    }
}   
