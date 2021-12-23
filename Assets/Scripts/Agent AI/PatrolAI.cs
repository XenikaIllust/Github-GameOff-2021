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
    [SerializeField] private List<Vector3> patrolPoints;
    private int _currentPatrolIndex;

    private void Awake()
    {
        _playerAgent = FindObjectOfType<PlayerAgent>();
        _unit = GetComponent<Unit>();
        _combatAIScript = GetComponent<AIAgent>();

        _combatAIScript.enabled = false;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, _playerAgent.transform.position) <= visionRange)
        {
            Vector2 vectorToPlayer = _playerAgent.transform.position - transform.position;
            var angleToPlayer = Mathf.Atan2(vectorToPlayer.y, vectorToPlayer.x) * Mathf.Rad2Deg;
            var angleToFront = _unit.PseudoObject.transform.eulerAngles.z;
            var deltaAngle = Mathf.Abs(angleToPlayer - angleToFront);

            if (deltaAngle <= visionAngle / 2)
            {
                enabled = false;
                _combatAIScript.enabled = true;
            }
        }
        else
        {
            if (patrolPoints.Count == 0) return;

            if (_unit.agent.remainingDistance <= Single.Epsilon)
            {
                _currentPatrolIndex += 1;
                if (_currentPatrolIndex > patrolPoints.Count) _currentPatrolIndex = 0;

                _unit.unitEventHandler.RaiseEvent("OnMoveOrderIssued", patrolPoints[_currentPatrolIndex]);
            }
        }
    }
}
