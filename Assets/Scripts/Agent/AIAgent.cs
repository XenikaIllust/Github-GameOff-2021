using System;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : Agent
{
    [Header("General Stats")] public float aggroRange = 5f;
    private bool _isAggro;
    protected Vector3 playerPosition;
    protected float distanceToPlayer = float.PositiveInfinity;
    protected List<float> abilityUtilities;
    protected List<float> damageSort;
    protected List<float> cooldownSort;
    protected float chasePlayerUtility;
    protected float stopUtility;

    protected override void Awake()
    {
        base.Awake();

        abilityUtilities = new List<float>(new float[4]);
        for (var i = 0; i < abilityUtilities.Count; i++) abilityUtilities[i] = -1;

        cooldownSort = new List<float>(new float[4]);
        for (var i = 0; i < cooldownSort.Count; i++)
        {
            cooldownSort[i] = thisUnit.abilities[i] == null ? float.NegativeInfinity : thisUnit.abilities[i].cooldown;
        }

        damageSort = new List<float>(new float[4]);
        for (var i = 0; i < damageSort.Count; i++)
        {
            damageSort[i] = thisUnit.abilities[i] == null ? float.NegativeInfinity : thisUnit.abilities[i].totalDamage;
        }

        cooldownSort.Sort();
        damageSort.Sort();
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
        distanceToPlayer = Vector3.Distance(transform.position, playerPosition);
        UtilityAI();
    }

    private void UtilityAI()
    {
        if (_isAggro == false)
        {
            if (distanceToPlayer > aggroRange) return;
            _isAggro = true;
        }

        CalculateUtility();
        CalculateRestriction();
        ExecuteBestAction();
    }

    protected virtual void CalculateUtility()
    {
        chasePlayerUtility = aggroRange - distanceToPlayer;
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

            if (thisUnit.abilityCooldowns[i] > float.Epsilon)
            {
                abilityUtilities[i] = float.NegativeInfinity;
                continue;
            }

            if (thisUnit.abilities[i].castRange < distanceToPlayer)
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