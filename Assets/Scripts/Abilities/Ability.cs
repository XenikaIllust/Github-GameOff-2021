using System.Collections.Generic;
using UnityEngine;

public enum AbilityType {TargetPoint, TargetUnit, TargetArea, NoTarget};

[System.Serializable]
[CreateAssetMenu(fileName = "New Ability", menuName = "Scriptable Objects/Ability/Ability")]
public class Ability : ScriptableObject
{
    public Sprite abilitySprite;
    public AbilityType InputType;
    public float Duration;
    public float Cooldown;
    public AbilityStatsDict AbilityStats;
    public Outcome[] Outcomes;
}
