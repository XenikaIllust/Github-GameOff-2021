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

    private void Awake()
    {
        unitEventHandler = GetComponent<UnitEventManager>().UnitEventHandler;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void OnEnable()
    {
        unitEventHandler.StartListening("OnMoveOrderIssued", OnMoveOrderIssued);
        EventManager.StartListening("OnStopOrderIssued", OnStopOrderIssued);
    }

    private void OnDisable()
    {
        EventManager.StopListening("OnMoveOrderIssued", OnMoveOrderIssued);
        EventManager.StopListening("OnStopOrderIssued", OnStopOrderIssued);
    }

    private void OnMoveOrderIssued(object destination)
    {
        agent.SetDestination((Vector3)destination);
    }

    private void OnStopOrderIssued(object arg0)
    {
        agent.SetDestination(transform.position);
    }
}