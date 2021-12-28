using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCollider : MonoBehaviour
{
    Unit unit;
    PatrolAI patrolAI;

    private void Start() {
        unit = transform.parent.gameObject.GetComponentInParent<Unit>();
        patrolAI = GetComponentInParent<PatrolAI>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log(other.transform.GetComponentInParent<Unit>());
        Debug.Log(unit);
        if(other.transform.GetComponentInParent<Unit>() && other.transform.GetComponentInParent<Unit>().alliance != unit.alliance) {
            // Calculate delta angle
            Vector2 vectorToTarget = other.transform.position - unit.transform.position;
            float angleToTarget = MathUtils.Mod((Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg), 360);
            float forwardAngle = MathUtils.Mod(unit.PseudoObject.transform.rotation.eulerAngles.z, 360);
            float deltaAngle = Mathf.Abs(angleToTarget - forwardAngle);

            if (deltaAngle <= patrolAI.visionAngle / 2)
            {
                patrolAI.spottedEnemy = true;
            }
        }
    }
}
