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
    List<object> targets; 

    public void ExecuteEffect(Unit self, Dictionary<string, object> InputTargets, Dictionary<string, List<object>> EffectTargets) {
        targets = TargetFilter.DetermineTargetUnits(self, InputTargets, EffectTargets);
        GameAction.Invoke(InputTargets, targets);
        EffectTargets[Id] = targets;
    }
}
