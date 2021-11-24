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
    [FormerlySerializedAs("AbilityStats")] public AbilityStatsDict abilityStats;
    [FormerlySerializedAs("Outcomes")] public Outcome[] outcomes;
}