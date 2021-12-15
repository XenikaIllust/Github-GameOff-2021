using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public enum Alliance
{
    TaciaAlliance,
    ClaireHorde
}

public class Unit : MonoBehaviour
{
    public EventProcessor unitEventHandler; // Internal event handler
    [Header("Stats")] public float baseMovementSpeed;
    public float baseTurnRate;
    [HideInInspector] public bool isPlayer;
    [HideInInspector] public NavMeshAgent agent;
    [Header("Misc.")] public Alliance alliance;
    [Range(0, 100)] public float bountyDropRate;
    private float _positionUpdateTimer;
    [Header("Abilities")] public List<Ability> abilities;
    [Header("Read Only")] public List<float> abilityCooldownList = new List<float>(new float[4]);
    public List<float> abilitySilenceDurationList = new List<float>(new float[4]);
    public float inputLockDuration;
    // public float ability1LockDuration;
    // public float ability2LockDuration;
    // public float ability3LockDuration;
    // public float ability4LockDuration;
    protected bool _isGamePaused;
    private Vector3 _castTargetPosition;
    private IEnumerator _pendingCast;
    private object _aiTarget;

    private float _abilityMovementSpeedMultiplier;
    private float _abilityTurnRateMultiplier;

    private float _movementSpeed;
    private float _turnRate;

    public GameObject PseudoObject { get; private set; }

    private void Update()
    {
        UpdateTimers();
        UpdatePlayerPosition();
        UpdateAnimationMovement();
    }

    private void Awake()
    {
        unitEventHandler = GetComponent<UnitEventManager>().UnitEventHandler;
        isPlayer = GetComponent<PlayerAgent>() != null;
        agent = GetComponent<NavMeshAgent>();

        _movementSpeed = baseMovementSpeed;
        _turnRate = baseTurnRate;

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = _movementSpeed;
        agent.acceleration = float.MaxValue;
        agent.angularSpeed = float.MaxValue;
        agent.autoBraking = true;

        while (abilities.Count < 4) abilities.Add(null);

        PseudoObject = new GameObject("PseudoObject")
        {
            transform = { parent = transform, localPosition = Vector3.zero }
        };
    }

    private void Start()
    {
        if (isPlayer) EventManager.RaiseEvent("OnPlayerSpawned", GetComponent<Unit>());
    }

    public virtual void OnEnable()
    {
        unitEventHandler.StartListening("OnMovementSpeedMultiplierChanged", OnMovementSpeedMultiplierChanged);
        unitEventHandler.StartListening("OnTurnRateMultiplierChanged", OnTurnRateMultiplierChanged);
        unitEventHandler.StartListening("OnStopOrderIssued", OnStopOrderIssued);
        unitEventHandler.StartListening("OnMoveOrderIssued", OnMoveOrderIssued);
        unitEventHandler.StartListening("OnLookOrderIssued", OnLookOrderIssued);
        unitEventHandler.StartListening("OnAbility1Casted", OnAbility1Casted);
        unitEventHandler.StartListening("OnAbility2Casted", OnAbility2Casted);
        unitEventHandler.StartListening("OnAbility3Casted", OnAbility3Casted);
        unitEventHandler.StartListening("OnAbility4Casted", OnAbility4Casted);
        unitEventHandler.StartListening("OnAbilityInputSet", OnAbilityInputSet);
        unitEventHandler.StartListening("OnInputLocked", OnInputLocked);
        unitEventHandler.StartListening("OnAbilityLocked", OnAbilityLocked);
        unitEventHandler.StartListening("OnDied", OnDied);

        EventManager.StartListening("OnGamePaused", OnGamePaused);
        EventManager.StartListening("OnGameResumed", OnGameResumed);
    }

    public virtual void OnDisable()
    {
        unitEventHandler.StopListening("OnMovementSpeedMultiplierChanged", OnMovementSpeedMultiplierChanged);
        unitEventHandler.StopListening("OnTurnRateMultiplierChanged", OnTurnRateMultiplierChanged);
        unitEventHandler.StopListening("OnStopOrderIssued", OnStopOrderIssued);
        unitEventHandler.StopListening("OnMoveOrderIssued", OnMoveOrderIssued);
        unitEventHandler.StopListening("OnLookOrderIssued", OnLookOrderIssued);
        unitEventHandler.StopListening("OnAbility1Casted", OnAbility1Casted);
        unitEventHandler.StopListening("OnAbility2Casted", OnAbility2Casted);
        unitEventHandler.StopListening("OnAbility3Casted", OnAbility3Casted);
        unitEventHandler.StopListening("OnAbility4Casted", OnAbility4Casted);
        unitEventHandler.StopListening("OnAbilityInputSet", OnAbilityInputSet);
        unitEventHandler.StopListening("OnInputLocked", OnInputLocked);
        unitEventHandler.StartListening("OnAbilityLocked", OnAbilityLocked);
        unitEventHandler.StopListening("OnDied", OnDied);

        EventManager.StopListening("OnGamePaused", OnGamePaused);
        EventManager.StopListening("OnGameResumed", OnGameResumed);
    }

    private void OnMovementSpeedMultiplierChanged(object movementSpeedMultiplier)
    {
        _abilityMovementSpeedMultiplier = (float)movementSpeedMultiplier;
        _movementSpeed = baseMovementSpeed * _abilityMovementSpeedMultiplier;

        agent.speed = _movementSpeed; // set the new movement speed
    }

    private void OnTurnRateMultiplierChanged(object turnRateMultiplier)
    {
        _abilityTurnRateMultiplier = (float)turnRateMultiplier;
        _turnRate = baseTurnRate * _abilityTurnRateMultiplier;
    }

    private void OnInputLocked(object duration)
    {
        inputLockDuration = Mathf.Max((float)duration, inputLockDuration);
    }

    private void OnAbilityLocked(object parameter)
    {
        var (abilityNumber, duration) = (Tuple<int, float>)parameter;

        switch (abilityNumber)
        {
            case 1:
                abilitySilenceDurationList[0] = Mathf.Max(duration, abilitySilenceDurationList[0]);
                break;
            case 2:
                abilitySilenceDurationList[1] = Mathf.Max(duration, abilitySilenceDurationList[1]);
                break;
            case 3:
                abilitySilenceDurationList[2] = Mathf.Max(duration, abilitySilenceDurationList[2]);
                break;
            case 4:
                abilitySilenceDurationList[3] = Mathf.Max(duration, abilitySilenceDurationList[3]);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnGamePaused(object @null)
    {
        _isGamePaused = true;
    }

    private void OnGameResumed(object @null)
    {
        _isGamePaused = false;
    }

    private void OnStopOrderIssued(object @null)
    {
        if (inputLockDuration > float.Epsilon) return;
        Stop();
    }

    private void OnMoveOrderIssued(object destination)
    {
        if (inputLockDuration > float.Epsilon) return;
        TurnAndMove((Vector3)destination);
    }

    private void OnLookOrderIssued(object target)
    {
        if (inputLockDuration > float.Epsilon) return;
        TurnAndLook((Vector3)target);
    }

    private void OnAbility1Casted(object target)
    {
        if (inputLockDuration > float.Epsilon) return;
        if (abilitySilenceDurationList[0] > float.Epsilon) return;
        AbilityCasted(target, 0);
    }

    private void OnAbility2Casted(object target)
    {
        if (inputLockDuration > float.Epsilon) return;
        if (abilitySilenceDurationList[1] > float.Epsilon) return;
        AbilityCasted(target, 1);
    }

    private void OnAbility3Casted(object target)
    {
        if (inputLockDuration > float.Epsilon) return;
        if (abilitySilenceDurationList[2] > float.Epsilon) return;
        AbilityCasted(target, 2);
    }

    private void OnAbility4Casted(object target)
    {
        if (inputLockDuration > float.Epsilon) return;
        if (abilitySilenceDurationList[3] > float.Epsilon) return;
        AbilityCasted(target, 3);
    }

    private void OnAbilityInputSet(object target)
    {
        AbilityInput(target);
    }

    private void OnDied(object @null)
    {
        OnDisable();
        Stop();
        Destroy(gameObject, 1f);
    }

    protected virtual void UpdateTimers()
    {
        inputLockDuration -= Time.deltaTime;
        for (var i = 0; i < abilitySilenceDurationList.Count; i++) abilitySilenceDurationList[i] -= Time.deltaTime;
        for (var i = 0; i < abilityCooldownList.Count; i++) abilityCooldownList[i] -= Time.deltaTime;
    }

    private void UpdatePlayerPosition()
    {
        if (!isPlayer) return;

        _positionUpdateTimer += Time.deltaTime;

        if (_positionUpdateTimer >= 0.1f)
        {
            _positionUpdateTimer = float.Epsilon;
            EventManager.RaiseEvent("OnPlayerPositionChanged", transform.position);
        }
    }

    private void Stop()
    {
        if (_pendingCast != null) StopCoroutine(_pendingCast);
        PseudoObject.transform.DOKill();
        agent.SetDestination(transform.position);
    }

    private void TurnAndMove(Vector3 destination)
    {
        Stop();

        PseudoObject.transform
            .DORotate(new Vector3(float.Epsilon, float.Epsilon, AngleToTarget(destination)), _turnRate * 360)
            .SetSpeedBased().SetEase(Ease.Linear).OnComplete(() => Move(destination));
    }

    private void Move(Vector3 destination)
    {
        agent.SetDestination(destination);
        unitEventHandler.RaiseEvent("OnPseudoObjectRotationChanged", PseudoObject.transform.rotation.eulerAngles.z);
    }

    private void TurnAndLook(Vector3 target)
    {
        Stop();

        PseudoObject.transform
            .DORotate(new Vector3(float.Epsilon, float.Epsilon, AngleToTarget(target)), _turnRate * 360).SetSpeedBased()
            .SetEase(Ease.Linear);

        unitEventHandler.RaiseEvent("OnPseudoObjectRotationChanged", PseudoObject.transform.rotation.eulerAngles.z);
    }

    private float AngleToTarget(Vector3 target)
    {
        Vector2 vectorToTarget = target - transform.position;
        return Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
    }

    // 'Q' Key
    public void OnFirstAbilityPressed(InputAction.CallbackContext context)
    {
        if (!isPlayer || _isGamePaused) return;

        if (context.started)
        {
            unitEventHandler.RaiseEvent("OnAbility1Casted", null);
        }
    }

    // 'W' Key
    public void OnSecondAbilityPressed(InputAction.CallbackContext context)
    {
        if (!isPlayer || _isGamePaused) return;

        if (context.started)
        {
            unitEventHandler.RaiseEvent("OnAbility2Casted", null);
        }
    }

    // 'E' Key
    public void OnThirdAbilityPressed(InputAction.CallbackContext context)
    {
        if (!isPlayer || _isGamePaused) return;

        if (context.started)
        {
            unitEventHandler.RaiseEvent("OnAbility3Casted", null);
        }
    }

    // 'R' Key
    public void OnFourthAbilityPressed(InputAction.CallbackContext context)
    {
        if (!isPlayer || _isGamePaused) return;

        if (context.started)
        {
            unitEventHandler.RaiseEvent("OnAbility4Casted", null);
        }
    }

    // members used for ability execution
    private AbilityType _currentAbilityType;
    private int _currentAbilityIndex;
    protected readonly Dictionary<string, object> _allTargets = new Dictionary<string, object>();

    private void AbilityCasted(object target, int index)
    {
        if (abilityCooldownList[index] <= float.Epsilon)
        {
            if (!isPlayer) _aiTarget = target;
            _currentAbilityIndex = index;
            StartCoroutine(CastAbility(abilities[index]));
        }
    }

    protected void AbilityInput(object target)
    {
        switch (_currentAbilityType)
        {
            case AbilityType.TargetPoint:
                _castTargetPosition = (Vector3)target;
                _allTargets["Target Point"] = _castTargetPosition;
                break;
            case AbilityType.TargetUnit:
                var targetUnit = (Unit)target;
                _castTargetPosition = targetUnit.transform.position;
                _allTargets["Target Unit"] = target;
                _allTargets["Target Unit Position"] = _castTargetPosition;
                break;
            case AbilityType.TargetArea:
                _castTargetPosition = (Vector3)target;
                _allTargets["Target Center"] = _castTargetPosition;
                break;
            case AbilityType.NoTarget:
                _castTargetPosition = (Vector3)target;
                _allTargets["Target Center"] = _castTargetPosition;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private IEnumerator CastAbility(Ability ability)
    {
        _currentAbilityType = ability.inputType;
        _allTargets.Clear();

        // by default, _allTargets should contain a reference to unit and a reference to unit position separately
        _allTargets["Executing Unit"] = this;
        _allTargets["Executing Unit Position"] = transform.position;

        if (_currentAbilityType != AbilityType.NoTarget)
        {
            if (isPlayer)
            {
                yield return StartCoroutine(GetComponent<PlayerAgent>().ProcessTargetInput(ability));
            }
            else
            {
                AbilityInput(_aiTarget);
            }
        }
        else
        {
            AbilityInput(_allTargets["Executing Unit Position"]);
        }

        if (Vector2.Distance(transform.position, _castTargetPosition) <= ability.castRange)
        {
            TurnAndExecuteAbility(ability);
        }
        else
        {
            TurnAndMove(_castTargetPosition);
            _pendingCast = PendingCast(ability);
            StartCoroutine(_pendingCast);
        }
    }

    private IEnumerator PendingCast(Ability ability)
    {
        while (Vector2.Distance(transform.position, _castTargetPosition) >= ability.castRange)
        {
            yield return null;
            agent.SetDestination(_castTargetPosition);
        }

        _pendingCast = null;
        TurnAndExecuteAbility(ability);
    }

    private void TurnAndExecuteAbility(Ability ability)
    {
        Stop();

        PseudoObject.transform
            .DORotate(new Vector3(float.Epsilon, float.Epsilon, AngleToTarget(_castTargetPosition)), _turnRate * 360)
            .SetSpeedBased().SetEase(Ease.Linear).OnComplete(() => StartCoroutine(ExecuteAbility(ability)));
    }

    private IEnumerator ExecuteAbility(Ability ability)
    {
        // Put the executed ability on cooldown
        abilityCooldownList[_currentAbilityIndex] = abilities[_currentAbilityIndex].cooldown;

        // Update the Player's rotation
        float eulerAnglesZ = PseudoObject.transform.rotation.eulerAngles.z;
        unitEventHandler.RaiseEvent("OnPseudoObjectRotationChanged", eulerAnglesZ);

        inputLockDuration = ability.castPoint + ability.castBackSwing;

        unitEventHandler.RaiseEvent("OnCastPointAnimating", ability.castPoint);
        yield return new WaitForSeconds(ability.castPoint);
        unitEventHandler.RaiseEvent("OnCastBackSwingAnimating", ability.castBackSwing);

        // used by AI to indicate ability has started execution
        unitEventHandler.RaiseEvent("OnAbilityStartedExecuting", null);

        foreach (var outcome in ability.outcomes)
        {
            var executionTime = outcome.Trigger.IsNormalizedTime
                ? outcome.Trigger.ExecutionTime * ability.duration
                : outcome.Trigger.ExecutionTime;

            StartCoroutine(ExecuteOutcome(outcome, ability, executionTime));
        }

        yield return null;
    }

    protected IEnumerator ExecuteOutcome(Outcome outcome, Ability ability, float timeToExecute)
    {
        yield return new WaitForSeconds(timeToExecute);
        foreach (var effect in outcome.Effects) effect.ExecuteEffect(ability.abilityStats, _allTargets);
        yield return null;
    }

    // Animation related functionality
    private float _speed;
    private Vector2 _lastPosition;
    private float _stopAnimationTimer;
    private AnimationType _currentAnimation;

    private enum AnimationType
    {
        Stop,
        Move
    }

    private void UpdateAnimationMovement()
    {
        if (isPlayer)
        {
            var position = transform.position;

            _speed = Mathf.Lerp(_speed, ((Vector2)position - _lastPosition).magnitude,
                0.3f /*adjust this number in order to make interpolation quicker or slower*/);
            _lastPosition = position;

            if (_speed < 0.0005f)
            {
                if (_currentAnimation == AnimationType.Stop) return;

                _stopAnimationTimer -= Time.deltaTime;
                if (_stopAnimationTimer <= float.Epsilon)
                {
                    _currentAnimation = AnimationType.Stop;
                    unitEventHandler.RaiseEvent("OnStopMoveAnimation", null);
                    unitEventHandler.RaiseEvent("OnPseudoObjectRotationChanged",
                        PseudoObject.transform.rotation.eulerAngles.z);
                }
            }
            else
            {
                if (_currentAnimation == AnimationType.Move) return;

                _currentAnimation = AnimationType.Move;
                _stopAnimationTimer = 2 / _turnRate;
                unitEventHandler.RaiseEvent("OnStartMoveAnimation", null);
                unitEventHandler.RaiseEvent("OnPseudoObjectRotationChanged",
                    PseudoObject.transform.rotation.eulerAngles.z);
            }
        }
    }
}
