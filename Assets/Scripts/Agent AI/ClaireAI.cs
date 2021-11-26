public class ClaireAI : AIAgent
{
    protected override void CalculateAbilityUtility()
    {
        for (var i = 0; i < abilityUtilities.Count; i++)
        {
            if (abilities[i] == null) continue;

            var rangeUtility = multiplier[i][0] * RangeFactor(idealRanges[i], idealRanges[i] * 2);
            var directionUtility = multiplier[i][1] * DirectionFactor(defaultBestAngle, defaultWorstAngle);
            var damageUtility = multiplier[i][2] * DamageFactor(abilities[i].totalDamage);
            var cooldownUtility = multiplier[i][3] * CooldownFactor(abilities[i].cooldown);

            abilityUtilities[i] = rangeUtility + directionUtility + damageUtility + cooldownUtility;
        }
    }
}