using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Definitions/Game Action/Teleport")]
public class TeleportActionBlock : GameActionBlock
{
    public override void Invoke(List<object> currentFilteredTargets, Dictionary<string, object> otherTargets)
    {
        // change position
        Unit unitToTeleport = (Unit) currentFilteredTargets[0];
        Vector2 teleportPos = (Vector3) otherTargets["Target Point"];

        NavMeshAgent navMeshAgent = unitToTeleport.GetComponent<NavMeshAgent>();

        Vector3 targetPosition = new Vector3(teleportPos.x, teleportPos.y, unitToTeleport.transform.position.z);
        unitToTeleport.transform.position = targetPosition;
        navMeshAgent.Warp(targetPosition);
    }
} 
