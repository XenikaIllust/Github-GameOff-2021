public class SwarmerAI : AIAgent
{
    protected override void CalculateUtility()
    {
        if (thisUnit.abilityCooldownList[0] <= float.Epsilon)
        {
            abilityUtilities[0] = float.PositiveInfinity * (thisUnit.abilities[0].castRange - distanceToTarget);
        }
        else
        {
            abilityUtilities[0] = float.NegativeInfinity;
        }

        chaseTargetUtility = 25;
        avoidUtility = 0;
        stopUtility = 0;
    }
}