public class SniperAI : AIAgent
{
    protected override void CalculateUtility()
    {
        if (thisUnit.cooldownTimers[0] <= float.Epsilon)
        {
            thisUnit.cooldownTimers[0] = thisUnit.abilities[0].cooldown;
            abilityUtilities[0] = float.PositiveInfinity * (thisUnit.abilities[0].castRange - distanceFromPlayer);
        }
        else
        {
            abilityUtilities[0] = float.NegativeInfinity;
        }

        chasePlayerUtility = distanceFromPlayer - aggroRange;
        stopUtility = 0;
    }
}