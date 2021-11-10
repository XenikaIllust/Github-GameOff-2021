using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// --------
///   Unit
/// --------
/// 
/// Basic framework for scene actors.
/// </summary>
public class Unit : MonoBehaviour
{
    // Event processor for handling internal messages
    EventProcessor unitEventHandler;

    [HideInInspector] public NavMeshAgent agent;
    [Header("Misc.")] public float positionUpdateInterval = 0.1f;
    private float _positionUpdateTimer;

    private void Awake()
    {
        unitEventHandler = GetComponent<UnitEventManager>().UnitEventHandler;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update() {
        UpdatePosition();
    }

    private void Start()
    {
        unitEventHandler.StartListening("OnMoveOrderIssued", OnMoveOrderIssued);
        unitEventHandler.StartListening("OnStopOrderIssued", OnStopOrderIssued);
    }

    private void OnDisable()
    {
        unitEventHandler.StopListening("OnMoveOrderIssued", OnMoveOrderIssued);
        unitEventHandler.StopListening("OnStopOrderIssued", OnStopOrderIssued);
    }

    private void OnMoveOrderIssued(object destination)
    {
        agent.SetDestination((Vector3)destination);
    }

    private void OnStopOrderIssued(object arg0)
    {
        agent.SetDestination(transform.position);
    }

    private void UpdatePosition()
    {
        _positionUpdateTimer += Time.deltaTime;

        if (_positionUpdateTimer >= positionUpdateInterval)
        {
            _positionUpdateTimer = float.Epsilon;
            EventManager.RaiseEvent("OnPlayerPositionChanged", transform.position);
        }
    }
}