using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Outcome
{
    public string Id;
    public Trigger Trigger;
    public Targeting Targeting;
    public GameAction[] Effects;
}
