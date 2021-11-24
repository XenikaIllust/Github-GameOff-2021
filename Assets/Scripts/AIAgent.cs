using System;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : Agent
{
    [Header("General Stats")] public float aggroRange = 5f;
    private Vector3 _playerPosition;
    protected float distanceFromPlayer;
    protected List<float> abilityCooldownTimers = new List<float>(new float[4]);
    protected List<float> abilityUtilities = new List<float>(new float[4]);
    protected float chasePlayerUtility;
    protected float stopUtility;

    private void Update()
    {
        for (var i = 0; i < abilityCooldownTimers.Count; i++)
        {
            abilityCooldownTimers[i] -= Time.deltaTime;
        }
    }

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
        _playerPosition = (Vector3)newPosition;
        distanceFromPlayer = Vector3.Distance(transform.position, _playerPosition);
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
        chasePlayerUtility = aggroRange - Vector3.Distance(transform.position, _playerPosition);
        stopUtility = 0;
    }

    private void CalculateRestriction()
    {
        for (var i = 0; i < abilityUtilities.Count; i++)
        {
            if (abilityCooldownTimers[i] > float.Epsilon ||
                thisUnit.abilities[i].castRange < Vector3.Distance(transform.position, _playerPosition))
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
        unitEventHandler.RaiseEvent("OnAbility1Casted", _playerPosition);
    }

    private void Ability2()
    {
        unitEventHandler.RaiseEvent("OnAbility2Casted", _playerPosition);
    }

    private void Ability3()
    {
        unitEventHandler.RaiseEvent("OnAbility3Casted", _playerPosition);
    }

    private void Ability4()
    {
        unitEventHandler.RaiseEvent("OnAbility4Casted", _playerPosition);
    }

    private void Chase()
    {
        unitEventHandler.RaiseEvent("OnMoveOrderIssued", _playerPosition);
    }

    private void Stop()
    {
        unitEventHandler.RaiseEvent("OnStopOrderIssued", null);
    }
}