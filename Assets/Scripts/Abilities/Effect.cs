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

    public void ExecuteEffect(Unit self, Dictionary<string, float> AbilityStats, Dictionary<string, object> AllTargets) {
        currentFilteredTargets = TargetFilter.DetermineTargetUnits(self, AbilityStats, AllTargets);
        GameAction.GameActionBlock.Invoke(AbilityStats, currentFilteredTargets, AllTargets);
        AllTargets[Id] = currentFilteredTargets;
    }
}
