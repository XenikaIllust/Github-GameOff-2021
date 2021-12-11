using Unity.Mathematics;
using UnityEngine;

public class MageAI : AIAgent
{
    [Header("Utility Stats")] [SerializeField] [Tooltip("The range this unit should go after kidnapping an ability")]
    protected float2 preferredEscapeRange = new float2 { x = 6f, y = 6f };

    bool abilityTaken = false;

    private void LateUpdate() {
        if(abilityTaken) {
            preferredCombatRange = preferredEscapeRange;
        }
    }

    protected override void CalculateAbilityUtility()
    {
        for (var i = 0; i < abilityUtilities.Count; i++)
        {
            if (abilities[i] == null) continue;

            var rangeUtility = abilityMultiplier[i][0] * RangeFactor(idealRanges[i], idealRanges[i] * 2);
            var directionUtility = abilityMultiplier[i][1] * DirectionFactor(defaultBestAngle, defaultWorstAngle);
            var damageUtility = abilityMultiplier[i][2] * DamageFactor(abilities[i].totalDamage);
            var cooldownUtility = abilityMultiplier[i][3] * CooldownFactor(abilities[i].cooldown);

            abilityUtilities[i] = rangeUtility + directionUtility + damageUtility + cooldownUtility;
        }
    }
}