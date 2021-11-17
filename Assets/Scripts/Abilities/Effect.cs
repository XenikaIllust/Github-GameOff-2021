using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effect
{
    public string Id;
    public TargetFilter TargetFilter;
    public GameAction GameAction;

    List<object> requiredInput;
    List<object> currentFilteredTargets; 

    public void ExecuteEffect(AbilityStatsDict AbilityStats, Dictionary<string, object> AllTargets) {
        currentFilteredTargets = TargetFilter.DetermineTargetUnits(AbilityStats, AllTargets);

        GameAction.Invoke(AbilityStats, currentFilteredTargets, AllTargets);

        AllTargets[Id] = currentFilteredTargets;
    }
}
