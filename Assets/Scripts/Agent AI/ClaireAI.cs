public class ClaireAI : AIAgent
{
    protected override void CalculateUtility()
    {
        // Assign utility value 
        abilityUtilities[0] = 5;
        abilityUtilities[1] = 4;
        abilityUtilities[2] = 3;
        abilityUtilities[3] = 2;
        chasePlayerUtility = 1;
        stopUtility = 0;
    }
}