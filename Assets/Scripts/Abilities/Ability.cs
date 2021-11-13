using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType {TargetPoint, TargetUnit, TargetArea, NoTarget};

[System.Serializable]
[CreateAssetMenu(fileName = "Ability", menuName = "Scriptable Objects/Ability/Ability SO")]
public class Ability : ScriptableObject
{
    public AbilityType InputType;
    public float Duration;
    public float Cooldown;
    public Outcome[] Outcomes;
}
