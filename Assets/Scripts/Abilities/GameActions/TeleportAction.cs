using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public struct TeleportActionData {
    public Unit unitToTeleport;
    public Vector2 teleportPos;

    public TeleportActionData(Unit u, Vector2 tpPos) {
        unitToTeleport = u;
        teleportPos = tpPos;
    }
}

[CreateAssetMenu(menuName = "Definitions/Game Action/Teleport")]
public class TeleportAction : GameAction
{
    public override void Invoke(object param)
    {
        Debug.Log(param);
        TeleportActionData teleportActionData = (TeleportActionData) param;

        // change position
        Unit unitToTeleport = teleportActionData.unitToTeleport;
        Vector2 teleportPos = teleportActionData.teleportPos;

        NavMeshAgent navMeshAgent = unitToTeleport.GetComponent<NavMeshAgent>();

        Vector3 targetPosition = new Vector3(teleportPos.x, teleportPos.y, unitToTeleport.transform.position.z);
        unitToTeleport.transform.position = targetPosition;
        navMeshAgent.Warp(targetPosition);
    }
} 
