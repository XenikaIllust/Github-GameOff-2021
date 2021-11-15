using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetFilterType {
		SelfFilter,
		UnitFilter,
		LineFilter,
		AOEFilter,
		TargetsOfPreviousEffect,
		NoFilter
	}

[System.Serializable]
public class TargetFilter
{
	// Target filters work by taking a set of potential targets and then by applying
	// a series of filters in turn to narrow down your targets until you're left
	// with who you want to target.

	public enum TargetRelationship
	{
		Hostile = 1 << 0,
		Neutral = 1 << 1,
		Friendly = 1 << 2,
	}

	public TargetFilterType Type;

	[Tooltip("Only fill in this field for Line TargetFilter")]
	public string InitialPointId;
	public string FinalPointId;

	[Tooltip("Only fill in this field for AOE TargetFilter")]
	public string AOERadiusId;
	public string TargetPointId;

	[Tooltip("Only fill in this field for TargetOfPreviousEffect TargetFilter")]
	public string PreviousEffectId;

	public TargetRelationship Relationship;

	public List<object> DetermineTargetUnits(Dictionary<string, float> AbilityStats, Dictionary<string, object> AllTargets) {
        List<object> targets = new List<object>();

		if(Type == TargetFilterType.SelfFilter) {
			// add self into targets
			targets.Add(AllTargets["Executing Unit"]);
		}
		else if(Type == TargetFilterType.UnitFilter) {
			targets.Add(AllTargets["Target Unit"]);
		}
		else if(Type == TargetFilterType.LineFilter) {
			// use raycast to get units in lineLength
		}
		else if(Type == TargetFilterType.AOEFilter) {
			// use OverlapCollider to get units in areaOfEffectRadius
			GameObject AOECalculator = new GameObject("AOECalculator", typeof(PolygonCollider2D));
			AOECalculator.transform.position = (Vector3) AllTargets["Target Center"];

			// firstly, construct an ellipse shaped collider with specified radius
			PolygonCollider2D polyCollider = AOECalculator.GetComponent<PolygonCollider2D>();
			polyCollider.points = MathUtils.GenerateIsometricCirclePoints(AbilityStats[AOERadiusId]);
			polyCollider.transform.position = (Vector3) AllTargets["Target Center"];

			// second, use the ellipse collider and check overlaps with existing hitbox colliders
			// return the targets
			List<Collider2D> results = new List<Collider2D>();
			ContactFilter2D contactFilter = new ContactFilter2D();
			Physics2D.OverlapCollider(polyCollider, contactFilter.NoFilter(), results);
			foreach(Collider2D collider in results) {
				Unit unit = collider.GetComponentInParent<Unit>();
				targets.Add(unit);
			}

			GameObject.Destroy(AOECalculator);
		}
		else if(Type == TargetFilterType.TargetsOfPreviousEffect) {
			// get targets from previous effect in EffectTargets[id] and add them into targets
			foreach(object o in (List<object>) AllTargets[PreviousEffectId]) {
				targets.Add(o);
			}
		}

        return targets;
    }
}
