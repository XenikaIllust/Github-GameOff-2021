using System;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : Agent
{
    [Header("General Stats")] public float aggroRange = 5f;
    protected Vector3 playerPosition;
    protected List<float> abilityUtilities = new List<float>(new float[4]);
    protected float chasePlayerUtility;
    protected float stopUtility;

    protected override void Awake()
    {
        base.Awake();

        for (var i = 0; i < abilityUtilities.Count; i++)
        {
            abilityUtilities[i] = -1;
        }
    }

    private void OnEnable()
    {
        EventManager.StartListening("OnPlayerPositionChanged", OnPlayerPositionChanged);
    }

    private void OnDisable()
    {
        EventManager.StopListening("OnPlayerPositionChanged", OnPlayerPositionChanged);
    }

    private void OnPlayerPositionChanged(object newPosition)
    {
        playerPosition = (Vector3)newPosition;
        UtilityAI();
    }

    private void UtilityAI()
    {
        CalculateUtility();
        CalculateRestriction();
        ExecuteBestAction();
    }

    protected virtual void CalculateUtility()
    {
        chasePlayerUtility = aggroRange - Vector3.Distance(transform.position, playerPosition);
        stopUtility = 0;
    }

    private void CalculateRestriction()
    {
        for (var i = 0; i < abilityUtilities.Count; i++)
        {
            if (thisUnit.abilities[i] == null)
            {
                abilityUtilities[i] = float.NegativeInfinity;
                continue;
            }

            if (thisUnit.cooldownTimers[i] > float.Epsilon ||
                thisUnit.abilities[i].castRange < Vector3.Distance(transform.position, playerPosition))
            {
                abilityUtilities[i] = float.NegativeInfinity;
            }
        }
    }

    private void ExecuteBestAction()
    {
        var list = new List<(Action, float)>
        {
            (Ability1, abilityUtilities[0]),
            (Ability2, abilityUtilities[1]),
            (Ability3, abilityUtilities[2]),
            (Ability4, abilityUtilities[3]),
            (Chase, chasePlayerUtility),
            (Stop, stopUtility)
        };

        list.Sort((x, y) => y.Item2.CompareTo(x.Item2));
        list[0].Item1.Invoke();
    }

    private void Ability1()
    {
        unitEventHandler.RaiseEvent("OnAbility1Casted", playerPosition);
    }

    private void Ability2()
    {
        unitEventHandler.RaiseEvent("OnAbility2Casted", playerPosition);
    }

    private void Ability3()
    {
        unitEventHandler.RaiseEvent("OnAbility3Casted", playerPosition);
    }

    private void Ability4()
    {
        unitEventHandler.RaiseEvent("OnAbility4Casted", playerPosition);
    }

    private void Chase()
    {
        unitEventHandler.RaiseEvent("OnMoveOrderIssued", playerPosition);
    }

    private void Stop()
    {
        unitEventHandler.RaiseEvent("OnStopOrderIssued", null);
    }
}