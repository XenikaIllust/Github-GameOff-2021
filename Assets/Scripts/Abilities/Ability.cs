using UnityEngine;
using UnityEngine.Serialization;

public enum AbilityType
{
    TargetPoint,
    TargetUnit,
    TargetArea,
    NoTarget
}

public enum AITargetPositionType
{
    OnTarget,
    BehindTarget,
    InFrontOfTarget
}

[System.Serializable]
[CreateAssetMenu(fileName = "New Ability", menuName = "Scriptable Objects/Ability/Ability")]
public class Ability : ScriptableObject
{
    public string abilityName;
    [TextArea] public string abilityDescription;
    public Sprite abilitySprite;
    public AbilityType inputType;
    public float duration;
    public float cooldown;
    public float castRange;
    public float castPoint = 0.3f;
    public float castBackSwing = 0.7f;

    [FormerlySerializedAs("potentialDamage")] [Header("Utility AI")]
    public float totalDamage;

    [Range(0, 200)]
    [Tooltip("0%: ideal when target at point blank range\n" +
             "100%: ideal up to max cast range\n" +
             ">100%: ideal up to more than max cast range")]
    public float idealRangePercentage = 100;

    public AITargetPositionType idealAITargetPosition = AITargetPositionType.OnTarget;
    public float targetPositionOffset = 1;

    [Space] public AbilityStatsDict abilityStats;
    public Outcome[] outcomes;
}