using System;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAI : MonoBehaviour
{
    private Unit _unit;
    private PlayerAgent _playerAgent;
    private AIAgent _combatAIScript;
    [SerializeField] private float visionRange;
    [Range(0, 360)] [SerializeField] private float visionAngle;
    [SerializeField] private List<Transform> patrolPoints;
    private int _currentPatrolIndex;

    ArcVFXController visionArcController;

    private void Awake()
    {
        _playerAgent = FindObjectOfType<PlayerAgent>();
        _unit = GetComponent<Unit>();
        _combatAIScript = GetComponent<AIAgent>();

        _combatAIScript.enabled = false;
    }

    private void Start() {
        visionArcController = Instantiate<ArcVFXController>(Resources.Load<ArcVFXController>("VFX/AbilityVFX/ArcVFX/ArcIndicator/VisionArcParent"), transform.position, Quaternion.identity, transform);
        visionArcController.arcWidth = visionAngle;
        MathUtils.ConvertCircleToIsometricCircle(visionRange, visionArcController.gameObject);
    }

    private void Update()
    {
        // TODO FIX BUG HERE
        if (Vector2.Distance(transform.position, _playerAgent.transform.position) <= visionRange)
        {
            Vector2 vectorToPlayer = _playerAgent.transform.position - transform.position;
            var angleToPlayer = Mathf.Atan2(vectorToPlayer.y, vectorToPlayer.x) * Mathf.Rad2Deg;
            var angleToFront = _unit.PseudoObject.transform.eulerAngles.z;
            var deltaAngle = Mathf.Abs(angleToPlayer - angleToFront);

            if (deltaAngle <= visionAngle / 2)
            {
                // destroy vision indicator
                Destroy(visionArcController.gameObject);

                enabled = false;
                _combatAIScript.enabled = true;
            }
        }
        else
        {
            if (patrolPoints.Count == 0) return;

            // the problem is here, this happens too many times
            if (_unit.agent.remainingDistance <= Single.Epsilon)
            {
                _currentPatrolIndex += 1;
                if (_currentPatrolIndex > patrolPoints.Count) _currentPatrolIndex = 0;

                Debug.Log("patrolPoints.Count " + patrolPoints.Count);
                Debug.Log("currentPatrolIndex " + _currentPatrolIndex);
                _unit.unitEventHandler.RaiseEvent("OnMoveOrderIssued", patrolPoints[_currentPatrolIndex].position);
            }
        }
    }
}
