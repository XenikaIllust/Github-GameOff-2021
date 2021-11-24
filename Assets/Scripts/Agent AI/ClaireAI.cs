using UnityEngine;

public class ClaireAI : AIAgent
{
    protected override void CalculateUtility()
    {
        var abilities = thisUnit.abilities;

        // Set multiplier respectively for Ability 1, 2, 3, 4
        var rangeMultiplier = new float[] { 25, 25, 25, 25 };
        var directionMultiplier = new float[] { 25, 25, 25, 25 };
        var damageMultiplier = new float[] { 25, 25, 25, 25 };
        var cooldownMultiplier = new float[] { 25, 25, 25, 25 };
        const float bestAngle = 180;
        const float worstAngle = 360;

        for (var i = 0; i < abilityUtilities.Count; i++)
        {
            var rangeUtility = rangeMultiplier[i] *
                               RangeFactor(abilities[i].idealRangePercentage, abilities[i].idealRangePercentage * 2);
            var directionUtility = directionMultiplier[i] * DirectionFactor(bestAngle, worstAngle);
            var damageUtility = damageMultiplier[i] * DamageFactor(abilities[i].potentialDamage);
            var cooldownUtility = cooldownMultiplier[i] * CooldownFactor(abilities[i].cooldown);

            abilityUtilities[i] = rangeUtility + directionUtility + damageUtility + cooldownUtility;
        }

        chasePlayerUtility = 50;
        stopUtility = 0;
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