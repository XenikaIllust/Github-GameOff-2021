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
    public AbilityType inputType;
    public float duration;
    public float cooldown;
    public float castRange;

    [FormerlySerializedAs("potentialDamage")] [Header("Utility AI")]
    public float totalDamage;

    [Range(0f, 200f)]
    [Tooltip("0% means the ability is ideal at point blank range, 100% means the ability is ideal at max cast range")]
    public float idealRangePercentage = 100f;

    [Space] public AbilityStatsDict abilityStats;
    public Outcome[] outcomes;
}