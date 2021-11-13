using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TargetFilter
{
	// Target filters work by taking a set of potential targets and then by applying
	// a series of filters in turn to narrow down your targets until you're left
	// with who you want to target.

	public enum TargetFilterType {
		SelfFilter,
		UnitFilter,
		LineFilter,
		AOEFilter,
		TargetsOfPreviousEffect,
		NoFilter
	}

	public enum TargetRelationship
	{
		Hostile = 1 << 0,
		Neutral = 1 << 1,
		Friendly = 1 << 2,
	}

	public TargetFilterType Type;

	[Tooltip("Only fill in this field for Line TargetFilter")]
	public float lineLength;

	[Tooltip("Only fill in this field for AOE TargetFilter")]
	public float areaOfEffectRadius;

	[Tooltip("Only fill in this field for TargetOfPreviousEffect TargetFilter")]
	public string IdOfPreviousEffect;

	public TargetRelationship Relationship;

	public List<object> DetermineTargetUnits(Unit self, Dictionary<string, object> InputTargets, Dictionary<string, List<object>> EffectTargets) {
        List<object> targets = new List<object>();

		if(Type == TargetFilterType.SelfFilter) {
			// add self into targets
			targets.Add(self);
		}
		else if(Type == TargetFilterType.UnitFilter) {
			targets.Add(InputTargets["Target Unit"]);
		}
		else if(Type == TargetFilterType.LineFilter) {
			// use raycast to get units in lineLength
		}
		else if(Type == TargetFilterType.AOEFilter) {
			// use OverlapSphere to get units in areaOfEffectRadius
		}
		else if(Type == TargetFilterType.TargetsOfPreviousEffect) {
			// get targets from previous effect in EffectTargets[id] and add them into targets
			foreach(object o in EffectTargets[IdOfPreviousEffect]) {
				targets.Add(o);
			}
		}

        return targets;
    }
}
