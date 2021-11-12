using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HealData {
    public int healAmount;

    public HealData(int heal) {
        healAmount = heal;
    }
}

public class HealAction : GameAction
{
    public override void Invoke(object param)
    {

    }
}
