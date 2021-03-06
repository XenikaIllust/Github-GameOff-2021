using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AIAgent : Agent
{
    [Header("General Stats")] [SerializeField]
    private float aggroRange = 5;

    private bool _isAggro;
    private readonly List<Vector3> _abilityTargetPosition = new List<Vector3>(new Vector3[4]);
    private readonly List<float> _damageSort = new List<float>(new float[4]);
    private readonly List<float> _cooldownSort = new List<float>(new float[4]);

    protected List<Ability> abilities = new List<Ability>(new Ability[4]);
    protected Vector3 targetPosition;
    protected float distanceToTarget = float.PositiveInfinity;
    protected readonly List<float> idealRanges = new List<float>(new float[4]);

    [Header("Utility Stats")] [SerializeField]
    private float2 preferredCombatRange = new float2 { x = 2.4f, y = 2.6f };

    [Range(0, 100)] [SerializeField] protected float basicMovementMultiplier = 50;
    [Range(0, 360)] [SerializeField] protected float defaultBestAngle = 180, defaultWorstAngle = 360;

    [Header("(Range, Direction, Damage, Cooldown)")] [SerializeField]
    protected List<float4> abilityMultiplier = new List<float4> { 25, 25, 25, 25 };

    [Header("Read Only")] [SerializeField] protected List<float> abilityUtilities = new List<float>(new float[4]);
    [SerializeField] private float chaseUtility;
    [SerializeField] private float avoidUtility;
    [SerializeField] private float lookUtility;
    [SerializeField] private float stopUtility;
    [Space] [SerializeField] private float forceChaseDuration;
    [SerializeField] private float forceAvoidDuration;
    [SerializeField] private float forceLookDuration;
    [SerializeField] private float forceStopDuration;

    private void Update()
    {
        forceChaseDuration -= Time.deltaTime;
        forceAvoidDuration -= Time.deltaTime;
        forceLookDuration -= Time.deltaTime;
        forceStopDuration -= Time.deltaTime;
    }

    protected override void Awake()
    {
        base.Awake();
        InitializeAILogic();
    }

    private void InitializeAILogic()
    {
        abilities = thisUnit.abilities;
        for (var i = 0; i < abilityUtilities.Count; i++) abilityUtilities[i] = 0;
        for (var i = 0; i < _cooldownSort.Count; i++)
            _cooldownSort[i] = abilities[i] == null ? float.NegativeInfinity : abilities[i].cooldown;
        for (var i = 0; i < _damageSort.Count; i++)
            _damageSort[i] = abilities[i] == null ? float.NegativeInfinity : abilities[i].totalDamage;

        for (var i = 0; i < idealRanges.Count; i++)
        {
            if (abilities[i] == null) return;

            idealRanges[i] = abilities[i].idealTargetPosition switch
            {
                AITargetPositionType.BehindTarget => abilities[i].castRange - abilities[i].targetPositionOffset,
                AITargetPositionType.InFrontOfTarget => abilities[i].castRange + abilities[i].targetPositionOffset,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        _cooldownSort.Sort();
        _damageSort.Sort();
    }

    private void OnEnable()
    {
        unitEventHandler.StartListening("OnChaseForced", OnChaseForced);
        unitEventHandler.StartListening("OnAvoidForced", OnAvoidForced);
        unitEventHandler.StartListening("OnLookForced", OnLookForced);
        unitEventHandler.StartListening("OnStopForced", OnStopForced);
        EventManager.StartListening("OnPlayerPositionChanged", OnPlayerPositionChanged);
    }

    private void OnDisable()
    {
        unitEventHandler.StopListening("OnChaseForced", OnChaseForced);
        unitEventHandler.StopListening("OnAvoidForced", OnAvoidForced);
        unitEventHandler.StopListening("OnLookForced", OnLookForced);
        unitEventHandler.StopListening("OnStopForced", OnStopForced);
        EventManager.StopListening("OnPlayerPositionChanged", OnPlayerPositionChanged);
    }

    private void OnChaseForced(object duration)
    {
        forceChaseDuration = Mathf.Max((float)duration, forceChaseDuration);
    }

    private void OnAvoidForced(object duration)
    {
        forceAvoidDuration = Mathf.Max((float)duration, forceAvoidDuration);
    }

    private void OnLookForced(object duration)
    {
        forceLookDuration = Mathf.Max((float)duration, forceLookDuration);
    }

    private void OnStopForced(object duration)
    {
        forceStopDuration = Mathf.Max((float)duration, forceStopDuration);
    }

    private void OnPlayerPositionChanged(object newPosition)
    {
        targetPosition = (Vector3)newPosition;
        distanceToTarget = Vector2.Distance(transform.position, targetPosition);
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
        CheckAbilityTargetPosition();
        CalculateRestriction();
        CalculateMovementUtility();
        ExecuteBestAction();
    }

    protected virtual void CalculateAbilityUtility()
    {
        Debug.LogError("Please override this method!");
    }

    private void CheckAbilityTargetPosition()
    {
        for (var i = 0; i < abilities.Count; i++)
        {
            var ability = abilities[i];

            if (ability != null)
            {
                switch (ability.idealTargetPosition)
                {
                    case AITargetPositionType.BehindTarget:
                        var outDirection = (targetPosition - transform.position).normalized;
                        _abilityTargetPosition[i] += outDirection * ability.targetPositionOffset;
                        break;
                    case AITargetPositionType.InFrontOfTarget:
                        var inDirection = (transform.position - targetPosition).normalized;
                        _abilityTargetPosition[i] += inDirection * ability.targetPositionOffset;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    private void CalculateRestriction()
    {
        for (var i = 0; i < abilityUtilities.Count; i++)
        {
            if (abilities[i] == null)
            {
                abilityUtilities[i] = float.NegativeInfinity;
                continue;
            }

            if (thisUnit.abilityCooldownList[i] > float.Epsilon)
            {
                abilityUtilities[i] = float.NegativeInfinity;
                continue;
            }

            var offset = abilities[i].idealTargetPosition switch
            {
                AITargetPositionType.BehindTarget => abilities[i].targetPositionOffset,
                AITargetPositionType.InFrontOfTarget => -abilities[i].targetPositionOffset,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (abilities[i].castRange < distanceToTarget + offset)
            {
                abilityUtilities[i] = 0;
            }
        }
    }

    private void CalculateMovementUtility()
    {
        chaseUtility = 0;
        avoidUtility = 0;
        lookUtility = 0;
        stopUtility = 1;

        var minRange = Mathf.Min(preferredCombatRange[0], preferredCombatRange[1]);
        var maxRange = Mathf.Max(preferredCombatRange[0], preferredCombatRange[1]);

        if (distanceToTarget < minRange)
        {
            avoidUtility = basicMovementMultiplier;
        }
        else if (minRange <= distanceToTarget && distanceToTarget <= maxRange)
        {
            lookUtility = basicMovementMultiplier;
        }
        else if (maxRange < distanceToTarget)
        {
            chaseUtility = basicMovementMultiplier;
        }

        if (forceChaseDuration > float.Epsilon) chaseUtility = float.PositiveInfinity;
        if (forceAvoidDuration > float.Epsilon) avoidUtility = float.PositiveInfinity;
        if (forceLookDuration > float.Epsilon) lookUtility = float.PositiveInfinity;
        if (forceStopDuration > float.Epsilon) stopUtility = float.PositiveInfinity;
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

    protected float RangeFactor(float bestRange, float worstRange)
    {
        if (distanceToTarget <= bestRange) return 1;
        if (distanceToTarget >= worstRange) return 0;
        return 1 - Mathf.Abs(distanceToTarget - bestRange) / Mathf.Abs(worstRange - bestRange);
    }

    protected float DirectionFactor(float bestAngle, float worstAngle)
    {
        Vector2 vectorToTarget = targetPosition - transform.position;
        var angleToTarget = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;

        if (angleToTarget <= bestAngle / 2) return 1;
        if (angleToTarget >= worstAngle / 2) return 0;
        return 1 - Mathf.Abs(angleToTarget - bestAngle / 2) / (Mathf.Abs(worstAngle - bestAngle) / 2);
    }

    protected float DamageFactor(float damage)
    {
        if (Mathf.Approximately(damage, _damageSort[0])) return 1;
        if (Mathf.Approximately(damage, _damageSort[1])) return 0.75f;
        if (Mathf.Approximately(damage, _damageSort[2])) return 0.50f;
        return 0.25f;
    }

    protected float CooldownFactor(float cooldown)
    {
        if (Mathf.Approximately(cooldown, _cooldownSort[0])) return 1;
        if (Mathf.Approximately(cooldown, _cooldownSort[1])) return 0.75f;
        if (Mathf.Approximately(cooldown, _cooldownSort[2])) return 0.50f;
        return 0.25f;
    }
}
