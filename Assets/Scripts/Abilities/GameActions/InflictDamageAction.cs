using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InflictDamageData {
    public int inflictedDamage;

    public InflictDamageData(int damage) {
        inflictedDamage = damage;
    }
}

public class InflictDamageAction : GameAction
{
    public override void Invoke(object param)
    {
        InflictDamageData inflictDamageData = (InflictDamageData) param;
    }
}
