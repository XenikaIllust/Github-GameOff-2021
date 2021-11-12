using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetingType
{
    Self,
    TargetFilter,
    TargetsOfPreviousOutcome,
}

[System.Serializable]
public class Targeting
{
    public TargetingType Type;
    public TargetFilter TargetFilter;
    public string PreviousOutcomeId;

    List<Unit> DetermineTargetUnits(Vector3 targetPoint) {
        List<Unit> targets = new List<Unit>();

        return targets;
    }

    List<Unit> DetermineTargetUnits() {
        List<Unit> targets = new List<Unit>();

        return targets;
    }
}
