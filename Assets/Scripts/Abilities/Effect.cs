using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effect
{
    public string Id;
    public TargetFilter TargetFilter;
    public GameAction GameAction;
    // public List<AbilityInput> RequiredInputs;

    List<object> requiredInput;
    List<object> targets; 

    void ExecuteEffect() {
        targets = TargetFilter.DetermineTargetUnits();
        GameAction.Invoke(targets);
    }
}
