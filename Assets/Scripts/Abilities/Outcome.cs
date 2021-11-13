using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Outcome
{
    public string Id;
    public float Duration;
    public Trigger Trigger;
    public Effect[] Effects;
}
