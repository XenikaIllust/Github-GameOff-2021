using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ClaireAI : AIAgent
{
    [Header("Default Utility Stats")] [Range(0, 100)] [SerializeField]
    private float chaseTarget = 25;

    [Range(0, 100)] [SerializeField] private float avoidTarget, stop;
    [Range(0, 360)] [SerializeField] private float defaultBestAngle = 180, defaultWorstAngle = 360;

    [Header("Utility Multiplier (Range, Direction, Damage, Cooldown)")] [SerializeField]
    private List<float4> multiplier = new List<float4> { 25, 25, 25, 25 };

    protected override void CalculateUtility()
    {
        var abilities = thisUnit.abilities;

        for (var i = 0; i < abilityUtilities.Count; i++)
        {
            if (abilities[i] == null) return;

            var rangeUtility = multiplier[i][0] * RangeFactor(
                abilities[i].castRange * abilities[i].idealRangePercentage / 100,
                abilities[i].castRange * abilities[i].idealRangePercentage / 100 * 2);
            var directionUtility = multiplier[i][1] * DirectionFactor(defaultBestAngle, defaultWorstAngle);
            var damageUtility = multiplier[i][2] * DamageFactor(abilities[i].totalDamage);
            var cooldownUtility = multiplier[i][3] * CooldownFactor(abilities[i].cooldown);

            abilityUtilities[i] = rangeUtility + directionUtility + damageUtility + cooldownUtility;
        }

        chaseTargetUtility = chaseTarget;
        avoidUtility = avoidTarget;
        stopUtility = stop;
    }

    private float RangeFactor(float bestRange, float worstRange)
    {
        if (distanceToTarget <= bestRange) return 1;
        if (distanceToTarget >= worstRange) return 0;
        return Mathf.Abs(distanceToTarget - bestRange) / Mathf.Abs(worstRange - bestRange);
    }

    private float DirectionFactor(float bestAngle, float worstAngle)
    {
        Vector2 vectorToTarget = targetPosition - transform.position;
        var angleToTarget = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;

        if (angleToTarget <= bestAngle / 2) return 1;
        if (angleToTarget >= worstAngle / 2) return 0;
        return Mathf.Abs(angleToTarget - bestAngle / 2) / (Mathf.Abs(worstAngle - bestAngle) / 2);
    }

    private float DamageFactor(float damage)
    {
        if (Mathf.Approximately(damage, damageSort[0])) return 1;
        if (Mathf.Approximately(damage, damageSort[1])) return 0.75f;
        if (Mathf.Approximately(damage, damageSort[2])) return 0.50f;
        return 0.25f;
    }

    private float CooldownFactor(float cooldown)
    {
        if (Mathf.Approximately(cooldown, cooldownSort[0])) return 1;
        if (Mathf.Approximately(cooldown, cooldownSort[1])) return 0.75f;
        if (Mathf.Approximately(cooldown, cooldownSort[2])) return 0.50f;
        return 0.25f;
    }
}