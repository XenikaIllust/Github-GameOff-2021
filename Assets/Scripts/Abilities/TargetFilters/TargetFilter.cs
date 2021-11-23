using System.Collections.Generic;
using UnityEngine;

public enum TargetFilterType
{
    SelfFilter,
    UnitFilter,
    LineFilter,
    AOEFilter,
    TargetsOfPreviousEffect,
    NoFilter,
    ConeFilter
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

    public string TargetCenterId;

    [Tooltip("Only fill in this field for TargetOfPreviousEffect TargetFilter")]
    public string PreviousEffectId;

    [Tooltip("Only fill in this field for Cone TargetFilter")]
    public string ConeRangeId;

    public string ConeAngleId;

    public string ConeTargetId;

    public TargetRelationship Relationship;

    public List<object> DetermineTargetUnits(SerializableDictionary<string, float> AbilityStats,
        Dictionary<string, object> AllTargets)
    {
        List<object> targets = new List<object>();

        if (Type == TargetFilterType.SelfFilter)
        {
            // add self into targets
            targets.Add(AllTargets["Executing Unit"]);
        }
        else if (Type == TargetFilterType.UnitFilter)
        {
            targets.Add(AllTargets["Target Unit"]);
        }
        else if (Type == TargetFilterType.LineFilter)
        {
            // use raycast to get units in lineLength
            LayerMask layerMask = LayerMask.GetMask("Enemy");
            Vector2 originPoint = (Vector3)AllTargets[InitialPointId];
            Vector2 finalPoint = (Vector3)AllTargets[FinalPointId];
            Vector2 direction = finalPoint - originPoint;
            float distance = direction.magnitude;

            RaycastHit2D[] hits =
                Physics2D.RaycastAll(originPoint, direction, distance: distance, layerMask: layerMask);

            foreach (RaycastHit2D hit in hits)
            {
                Unit targetUnit = hit.collider.GetComponentInParent<Unit>();
                targets.Add(targetUnit);
            }
        }
        else if (Type == TargetFilterType.AOEFilter)
        {
            // use OverlapCollider to get units in areaOfEffectRadius
            GameObject AOECalculator = new GameObject("AOECalculator", typeof(PolygonCollider2D));
            AOECalculator.transform.position = (Vector3)AllTargets[TargetCenterId];

            // firstly, construct an ellipse shaped collider with specified radius
            PolygonCollider2D polyCollider = AOECalculator.GetComponent<PolygonCollider2D>();
            polyCollider.points = MathUtils.GenerateIsometricCirclePoints(AbilityStats[AOERadiusId]);
            polyCollider.transform.position = (Vector3)AllTargets[TargetCenterId];

            // second, use the ellipse collider and check overlaps with existing hitbox colliders
            // return the targets
            List<Collider2D> results = new List<Collider2D>();
            ContactFilter2D contactFilter = new ContactFilter2D();
            Physics2D.OverlapCollider(polyCollider, contactFilter.NoFilter(), results);
            foreach (Collider2D collider in results)
            {
                Unit unit = collider.GetComponentInParent<Unit>();
                targets.Add(unit);
            }

            // GameObject.Destroy(AOECalculator);
        }
        else if (Type == TargetFilterType.TargetsOfPreviousEffect)
        {
            // get targets from previous effect in EffectTargets[id] and add them into targets
            foreach (object o in (List<object>)AllTargets[PreviousEffectId])
            {
                targets.Add(o);
            }
        }
        else if (Type == TargetFilterType.ConeFilter)
        {
            Vector3 executingUnitPosition = (Vector3)AllTargets["Executing Unit Position"];
            Vector3 targetCenter = (Vector3)AllTargets[ConeTargetId];

            // use OverlapCollider to get units in areaOfEffectRadius
            GameObject AOECalculator = new GameObject("AOECalculator", typeof(PolygonCollider2D));
            AOECalculator.transform.position = (Vector3)AllTargets["Executing Unit Position"];

            // firstly, construct an ellipse shaped collider with specified radius
            PolygonCollider2D polyCollider = AOECalculator.GetComponent<PolygonCollider2D>();
            polyCollider.points = MathUtils.GenerateIsometricCirclePoints(AbilityStats[ConeRangeId]);
            polyCollider.transform.position = (Vector3)AllTargets["Executing Unit Position"];

            // second, use the ellipse collider and check overlaps with existing hitbox colliders
            // return the targets
            List<Collider2D> results = new List<Collider2D>();
            ContactFilter2D contactFilter = new ContactFilter2D();
            Physics2D.OverlapCollider(polyCollider, contactFilter.NoFilter(), results);
            foreach (Collider2D collider in results)
            {
                Unit unit = collider.GetComponentInParent<Unit>();

                // Calculate delta angle
                Vector2 vectorToTarget = targetCenter - executingUnitPosition;
                Vector2 vectorToUnit = unit.transform.position - executingUnitPosition;
                float angleToTarget = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                float angleToUnit = Mathf.Atan2(vectorToUnit.y, vectorToUnit.x) * Mathf.Rad2Deg;
                float deltaAngle = Mathf.Abs(angleToTarget - angleToUnit);

                if (deltaAngle <= AbilityStats[ConeAngleId] / 2 && unit != (Unit)AllTargets["Executing Unit"])
                {
                    targets.Add(unit);
                }
            }

            GameObject.Destroy(AOECalculator);
        }

        return targets;
    }
}