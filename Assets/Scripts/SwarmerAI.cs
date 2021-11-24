public class SwarmerAI : AIAgent
{
    protected override void CalculateUtility()
    {
        if (abilityCooldownTimers[0] <= float.Epsilon)
        {
            abilityCooldownTimers[0] = thisUnit.abilities[0].cooldown;
            abilityUtilities[0] = float.PositiveInfinity * (thisUnit.abilities[0].castRange - distanceFromPlayer);
        }
        else
        {
            abilityUtilities[0] = float.NegativeInfinity;
        }
    }
}