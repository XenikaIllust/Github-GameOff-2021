using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Definitions/Game Action/Teleport")]
public class TeleportAction : GameAction
{
    public override void Invoke(Dictionary<string, object> inputTargets, List<object> targets)
    {
        // change position
        Unit unitToTeleport = (Unit) targets[0];
        Vector2 teleportPos = (Vector3) inputTargets["Target Point"];

        NavMeshAgent navMeshAgent = unitToTeleport.GetComponent<NavMeshAgent>();

        Vector3 targetPosition = new Vector3(teleportPos.x, teleportPos.y, unitToTeleport.transform.position.z);
        unitToTeleport.transform.position = targetPosition;
        navMeshAgent.Warp(targetPosition);
    }
} 
