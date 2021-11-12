using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModifierType {
    MODIFIER_TURNSPEED
}

[CreateAssetMenu(menuName = "Definitions/Game Action/Add Modifier")]
public class AddModifierAction : GameAction
{
    public ModifierType modifierType;

    public override void Invoke(object param)
    {
        
    }
}
