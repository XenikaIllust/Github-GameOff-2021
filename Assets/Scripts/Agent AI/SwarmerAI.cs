public class SwarmerAI : AIAgent
{
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