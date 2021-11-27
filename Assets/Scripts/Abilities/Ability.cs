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

    public AITargetPositionType idealTargetPosition = AITargetPositionType.BehindTarget;
    public float targetPositionOffset = 0.1f;

    [Space] public AbilityStatsDict abilityStats;
    public Outcome[] outcomes;
}