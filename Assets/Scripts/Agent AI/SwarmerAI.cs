public class SwarmerAI : AIAgent
{
    protected override void CalculateUtility()
    {
        if (thisUnit.abilityCooldowns[0] <= float.Epsilon)
        {
            abilityUtilities[0] = float.PositiveInfinity * (thisUnit.abilities[0].castRange - distanceToPlayer);
        }
        else
        {
            abilityUtilities[0] = float.NegativeInfinity;
        }

        chasePlayerUtility = aggroRange - distanceToPlayer;
        stopUtility = 0;
    }
}