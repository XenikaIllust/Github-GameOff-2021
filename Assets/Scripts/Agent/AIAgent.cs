using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AIAgent : Agent
{
    [Header("General Stats")] public float aggroRange = 5;
    [Header("Min, Max")] public float2 preferredCombatRange = new float2 { x = 2.4f, y = 2.6f };
    private bool _isAggro;
    private bool _allAbilityOnCooldown;
    protected Vector3 targetPosition;
    protected float distanceToTarget = float.PositiveInfinity;
    private readonly List<Vector3> _abilityTargetPosition = new List<Vector3>(new Vector3[4]);
    protected readonly List<float> abilityUtilities = new List<float>(new float[4]);
    protected readonly List<float> damageSort = new List<float>(new float[4]);
    protected readonly List<float> cooldownSort = new List<float>(new float[4]);
    protected float chaseUtility;
    protected float avoidUtility;
    protected float lookUtility;
    protected float stopUtility;

    protected override void Awake()
    {
        base.Awake();
        InitializeUtilityLists();
    }

    private void InitializeUtilityLists()
    {
        for (var i = 0; i < abilityUtilities.Count; i++) abilityUtilities[i] = 0;
        for (var i = 0; i < cooldownSort.Count; i++)
            cooldownSort[i] = thisUnit.abilities[i] == null ? float.NegativeInfinity : thisUnit.abilities[i].cooldown;
        for (var i = 0; i < damageSort.Count; i++)
            damageSort[i] = thisUnit.abilities[i] == null ? float.NegativeInfinity : thisUnit.abilities[i].totalDamage;

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
        targetPosition = (Vector3)newPosition;
        distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        for (var i = 0; i < _abilityTargetPosition.Count; i++) _abilityTargetPosition[i] = targetPosition;
        AISequence();
    }

    private void AISequence()
    {
        if (_isAggro == false)
        {
            if (distanceToTarget > aggroRange) return;
            _isAggro = true;
        }

        CalculateAbilityUtility();
        CalculateMovementUtility();
        CheckRestrictionException();
        CalculateRestriction();
        if (_allAbilityOnCooldown) RecalculateMovementUtility();
        ExecuteBestAction();
    }

    protected virtual void CalculateAbilityUtility()
    {
        Debug.LogError("Please override this method!");
    }

    private void CalculateMovementUtility()
    {
        chaseUtility = 25;
        avoidUtility = 0;
        lookUtility = 0;
        stopUtility = 0;
    }

    private void CheckRestrictionException()
    {
        for (var i = 0; i < thisUnit.abilities.Count; i++)
        {
            var ability = thisUnit.abilities[i];

            if (ability != null)
            {
                switch (ability.idealAITargetPosition)
                {
                    case AITargetPositionType.OnTarget:
                        break;
                    case AITargetPositionType.BehindTarget:
                        var outDirection = (_abilityTargetPosition[i] - transform.position).normalized;
                        _abilityTargetPosition[i] += outDirection * ability.targetPositionOffset;
                        break;
                    case AITargetPositionType.InFrontOfTarget:
                        var inDirection = (transform.position - _abilityTargetPosition[i]).normalized;
                        _abilityTargetPosition[i] += inDirection * ability.targetPositionOffset;
                        if (distanceToTarget < ability.targetPositionOffset * 2)
                            abilityUtilities[i] = float.NegativeInfinity;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    private void CalculateRestriction()
    {
        var abilityCount = 0;
        var abilityOnCooldownCount = 0;

        for (var i = 0; i < abilityUtilities.Count; i++)
        {
            if (thisUnit.abilities[i] == null)
            {
                abilityUtilities[i] = float.NegativeInfinity;
                continue;
            }

            abilityCount += 1;
            if (thisUnit.abilityCooldownList[i] > float.Epsilon)
            {
                abilityUtilities[i] = float.NegativeInfinity;
                abilityOnCooldownCount += 1;
                continue;
            }

            if (thisUnit.abilities[i].castRange < distanceToTarget)
            {
                abilityUtilities[i] = float.NegativeInfinity;
            }
        }

        _allAbilityOnCooldown = abilityOnCooldownCount == abilityCount;
    }

    private void RecalculateMovementUtility()
    {
        var minRange = Mathf.Min(preferredCombatRange[0], preferredCombatRange[1]);
        var maxRange = Mathf.Max(preferredCombatRange[0], preferredCombatRange[1]);

        if (distanceToTarget < minRange)
        {
            avoidUtility = 100;
        }
        else if (minRange <= distanceToTarget && distanceToTarget <= maxRange)
        {
            lookUtility = 100;
        }
        else if (maxRange < distanceToTarget)
        {
            chaseUtility = 100;
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
            (Chase, chaseUtility),
            (Avoid, avoidUtility),
            (Look, lookUtility),
            (Stop, stopUtility)
        };

        list.Sort((x, y) => y.Item2.CompareTo(x.Item2));
        list[0].Item1.Invoke();
    }

    private void Ability1()
    {
        unitEventHandler.RaiseEvent("OnAbility1Casted", _abilityTargetPosition[0]);
    }

    private void Ability2()
    {
        unitEventHandler.RaiseEvent("OnAbility2Casted", _abilityTargetPosition[1]);
    }

    private void Ability3()
    {
        unitEventHandler.RaiseEvent("OnAbility3Casted", _abilityTargetPosition[2]);
    }

    private void Ability4()
    {
        unitEventHandler.RaiseEvent("OnAbility4Casted", _abilityTargetPosition[3]);
    }

    private void Chase()
    {
        unitEventHandler.RaiseEvent("OnMoveOrderIssued", targetPosition);
    }

    private void Avoid()
    {
        var thisPosition = transform.position;
        var targetOpposition = thisPosition - (targetPosition - thisPosition);
        unitEventHandler.RaiseEvent("OnMoveOrderIssued", targetOpposition);
    }

    private void Look()
    {
        unitEventHandler.RaiseEvent("OnLookOrderIssued", targetPosition);
    }

    private void Stop()
    {
        unitEventHandler.RaiseEvent("OnStopOrderIssued", null);
    }
}