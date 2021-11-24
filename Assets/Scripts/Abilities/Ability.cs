using System;
using UnityEngine;
using UnityEngine.Serialization;

public enum AbilityType
{
    TargetPoint,
    TargetUnit,
    TargetArea,
    NoTarget
}

[System.Serializable]
[CreateAssetMenu(fileName = "New Ability", menuName = "Scriptable Objects/Ability/Ability")]
public class Ability : ScriptableObject
{
    public Sprite abilitySprite;
    [FormerlySerializedAs("InputType")] public AbilityType inputType;
    [FormerlySerializedAs("Duration")] public float duration;
    [FormerlySerializedAs("Cooldown")] public float cooldown;
    public float castRange;

    [Header("Utility AI")] public float potentialDamage;

    [Range(0f, 100f)]
    [Tooltip("0% means the ability is ideal at point blank range, 100% means the ability is ideal at max cast range")]
    public float idealRangePercentage = 100f;

    [Space] [FormerlySerializedAs("AbilityStats")]
    public AbilityStatsDict abilityStats;

    [FormerlySerializedAs("Outcomes")] public Outcome[] outcomes;
}