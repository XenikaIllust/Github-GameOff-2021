using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType { TargetPoint, TargetUnit, TargetArea, NoTarget }

[CreateAssetMenu(fileName = "New Ability", menuName = "Scriptable Objects/Ability/Ability Config")]
public class AbilityConfigSO : ScriptableObject
{
    [SerializeField] public string abilityName;
    [SerializeField] Sprite abilityIcon;
    [SerializeField] AbilityType abilityType;

    void ExecuteAbility() {

    }
}
