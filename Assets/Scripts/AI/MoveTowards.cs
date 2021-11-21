using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class MoveTowardsPlayer : Action
{
    EventProcessor unitEventHandler;
    Unit self;
    Unit playerUnit;

    public override void OnAwake()
    {
        unitEventHandler = GetComponent<UnitEventManager>().UnitEventHandler;
        self = GetComponent<Unit>();

        // cache the player
        playerUnit = GameObject.FindObjectOfType<PlayerAgent>().GetComponent<Unit>();
    }

    public override TaskStatus OnUpdate()
    {
        unitEventHandler.RaiseEvent("OnMoveOrderIssued", playerUnit.transform.position);
        return TaskStatus.Success;
    }
}
