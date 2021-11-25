using Unity.Mathematics;
using UnityEngine;

public class ClaireAI : AIAgent
{
    [Header("Utility Stats")] [Range(0f, 100f)] [SerializeField]
    private float chasePlayer = 25;

    [Range(0f, 100f)] [SerializeField] private float stop;

    [Header("Utility Multiplier (Range, Direction, Damage, Cooldown)")] [SerializeField]
    private float4[] multiplier = { 25, 25, 25, 25 };

    protected override void CalculateUtility()
    {
        var abilities = thisUnit.abilities;

        const float defaultBestAngle = 180;
        const float defaultWorstAngle = 360;

        for (var i = 0; i < abilityUtilities.Count; i++)
        {
            var rangeUtility = multiplier[i][1] * RangeFactor(
                abilities[i].castRange * abilities[i].idealRangePercentage / 100,
                abilities[i].castRange * abilities[i].idealRangePercentage / 100 * 2);
            var directionUtility = multiplier[i][2] * DirectionFactor(defaultBestAngle, defaultWorstAngle);
            var damageUtility = multiplier[i][3] * DamageFactor(abilities[i].totalDamage);
            var cooldownUtility = multiplier[i][4] * CooldownFactor(abilities[i].cooldown);

            abilityUtilities[i] = rangeUtility + directionUtility + damageUtility + cooldownUtility;
        }

        chasePlayerUtility = chasePlayer;
        stopUtility = stop;
    }

    private float RangeFactor(float bestRange, float worstRange)
    {
        if (distanceToPlayer <= bestRange) return 1;
        if (distanceToPlayer >= worstRange) return 0;
        return Mathf.Abs(distanceToPlayer - bestRange) / Mathf.Abs(worstRange - bestRange);
    }

    private float DirectionFactor(float bestAngle, float worstAngle)
    {
        Vector2 vectorToTarget = playerPosition - transform.position;
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