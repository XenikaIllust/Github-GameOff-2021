using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAI : MonoBehaviour
{
    private Unit _unit;
    private PlayerAgent _playerAgent;
    private AIAgent _combatAIScript;
    [SerializeField] private float visionRange;
    [Range(0, 360)] public float visionAngle;
    [SerializeField] private List<Transform> patrolPoints;
    private int _currentPatrolIndex = 0;
    Vector3 currentDestination;

    bool movingToWaypoint = false;

    ArcVFXController visionArcController;
    GameObject visionColliderGO;

    public bool spottedEnemy = false;

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

        visionColliderGO = new GameObject("VisionCollider", typeof(PolygonCollider2D), typeof(VisionCollider));
        visionColliderGO.transform.position = transform.position;
        visionColliderGO.transform.SetParent(visionArcController.transform, true);
        visionColliderGO.transform.localScale = Vector3.one;
        
        PolygonCollider2D visionCollider = visionColliderGO.GetComponent<PolygonCollider2D>();
        visionCollider.points = MathUtils.GenerateIsometricCirclePoints(visionRange);

        MathUtils.ConvertCircleToIsometricCircle(visionRange, visionArcController.transform.GetChild(0).gameObject);

        StartCoroutine(SpotEnemy());
    }

    private void Update()
    {
        // TODO FIX BUG HERE
        if (patrolPoints.Count == 0) return;

        if(!movingToWaypoint) {
            StartCoroutine(MoveToNextWaypoint());
        }
    }

    private IEnumerator MoveToNextWaypoint() {
        Vector3 waypoint = new Vector3(patrolPoints[_currentPatrolIndex].position.x, patrolPoints[_currentPatrolIndex].position.y, transform.position.z); // set Z position to be same as unit
        _unit.unitEventHandler.RaiseEvent("OnMoveOrderIssued", waypoint);
        movingToWaypoint = true;

        yield return new WaitForSeconds(3f);

        yield return new WaitUntil(() => _unit.agent.remainingDistance <= Single.Epsilon);
        _currentPatrolIndex = (_currentPatrolIndex + 1) % patrolPoints.Count;
        movingToWaypoint = false;
    }

    private IEnumerator SpotEnemy() {
        // change this code to use colliders
        // yield return new WaitUntil(() => Vector2.Distance(transform.position, _playerAgent.transform.position) <= visionRange);

        // Vector2 vectorToPlayer = _playerAgent.transform.position - transform.position;
        // var angleToPlayer = Mathf.Atan2(vectorToPlayer.y, vectorToPlayer.x) * Mathf.Rad2Deg;
        // var angleToFront = _unit.PseudoObject.transform.eulerAngles.z;
        // var deltaAngle = Mathf.Abs(angleToPlayer - angleToFront);

        // if (deltaAngle <= visionAngle / 2)
        // {
        //     // destroy vision indicator
        //     Destroy(visionArcController.gameObject);

        //     enabled = false;
        //     _combatAIScript.enabled = true;
        // }

        yield return new WaitUntil(() => spottedEnemy);
        // destroy vision indicator
        Destroy(visionArcController.gameObject);

        enabled = false;
        _combatAIScript.enabled = true;
    }
}
