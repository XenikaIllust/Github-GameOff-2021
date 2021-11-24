using UnityEngine;

public class SwarmerAI : AIAgent
{
    protected override void CalculateUtility()
    {
        if (thisUnit.abilityCooldowns[0] <= float.Epsilon)
        {
            abilityUtilities[0] = float.PositiveInfinity *
                                  (thisUnit.abilities[0].castRange -
                                   Vector3.Distance(transform.position, playerPosition));
        }
        else
        {
            abilityUtilities[0] = float.NegativeInfinity;
        }

        chasePlayerUtility = aggroRange - Vector3.Distance(transform.position, playerPosition);
        stopUtility = 0;
    }
}