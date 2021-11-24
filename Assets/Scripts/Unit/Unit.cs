using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

/// <summary>
/// --------
///   Unit
/// --------
/// 
/// Basic framework for scene actors.
/// </summary>
public class Unit : MonoBehaviour
{
    public EventProcessor unitEventHandler; // Internal event handler
    [Header("Stats")] public float movementSpeed = 3.5f;
    [HideInInspector] public float defaultMovementSpeed;
    public float turnRate = 5f;
    [HideInInspector] public float defaultTurnRate;
    [HideInInspector] public bool isPlayer;
    [HideInInspector] public NavMeshAgent agent;
    [Header("Misc.")] public Alliance alliance;
    private float _positionUpdateTimer;
    [Header("Abilities")] [SerializeField] public Ability[] abilities = new Ability[4];
    private Vector3 _castTargetPosition;
    private IEnumerator _pendingCast;
    private object _aiTarget;

    public enum Alliance
    {
        TaciaAlliance,
        ClaireHorde
    }

    public GameObject PseudoObject { get; private set; }

    private void Awake()
    {
        unitEventHandler = GetComponent<UnitEventManager>().UnitEventHandler;
        isPlayer = GetComponent<PlayerAgent>() != null;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = movementSpeed;
        agent.acceleration = float.MaxValue;
        agent.angularSpeed = float.MaxValue;
        agent.autoBraking = true;

        PseudoObject = new GameObject("PseudoObject")
        {
            transform =
            {
                parent = transform
            }
        };

        defaultMovementSpeed = movementSpeed;
        defaultTurnRate = turnRate;
    }

    private void OnEnable()
    {
        unitEventHandler.StartListening("OnStopOrderIssued", OnStopOrderIssued);
        unitEventHandler.StartListening("OnMoveOrderIssued", OnMoveOrderIssued);
        unitEventHandler.StartListening("OnDied", OnDied);
        unitEventHandler.StartListening("OnAbility1Casted", OnAbility1Casted);
        unitEventHandler.StartListening("OnAbility2Casted", OnAbility2Casted);
        unitEventHandler.StartListening("OnAbility3Casted", OnAbility3Casted);
        unitEventHandler.StartListening("OnAbility4Casted", OnAbility4Casted);

        unitEventHandler.StartListening("OnAbilityInputSet", OnAbilityInputSet);
    }

    private void OnDisable()
    {
        unitEventHandler.StopListening("OnStopOrderIssued", OnStopOrderIssued);
        unitEventHandler.StopListening("OnMoveOrderIssued", OnMoveOrderIssued);
        unitEventHandler.StopListening("OnDied", OnDied);
        unitEventHandler.StopListening("OnAbility1Casted", OnAbility1Casted);
        unitEventHandler.StopListening("OnAbility2Casted", OnAbility2Casted);
        unitEventHandler.StopListening("OnAbility3Casted", OnAbility3Casted);
        unitEventHandler.StopListening("OnAbility4Casted", OnAbility4Casted);

        unitEventHandler.StopListening("OnAbilityInputSet", OnAbilityInputSet); // temporary for testing
    }

    private void OnStopOrderIssued(object @null)
    {
        Stop();
    }

    private void OnMoveOrderIssued(object destination)
    {
        TurnAndMove((Vector3)destination);
    }

    private void OnAbility1Casted(object target)
    {
        if (!isPlayer) _aiTarget = target;
        StartCoroutine(CastAbility(abilities[0]));
    }

    private void OnAbility2Casted(object target)
    {
        if (!isPlayer) _aiTarget = target;
        StartCoroutine(CastAbility(abilities[1]));
    }

    private void OnAbility3Casted(object target)
    {
        if (!isPlayer) _aiTarget = target;
        StartCoroutine(CastAbility(abilities[2]));
    }

    private void OnAbility4Casted(object target)
    {
        if (!isPlayer) _aiTarget = target;
        StartCoroutine(CastAbility(abilities[3]));
    }

    private void OnAbilityInputSet(object target)
    {
        AbilityInput(target);
    }

    private void OnDied(object @null)
    {
        OnDisable();
        Stop();
        Destroy(gameObject, 1f); // edit by rin , wanna to have 1s for unit dead vfx animation
    }

    private void Update()
    {
        UpdatePlayerPosition();
        UpdateAnimationMovement();
    }

    // 'Q' Key
    public void OnFirstAbilityPressed(InputAction.CallbackContext context)
    {
        if (!isPlayer) return;

        if (context.canceled)
        {
            unitEventHandler.RaiseEvent("OnAbility1Casted", null);
        }
    }

    // 'W' Key
    public void OnSecondAbilityPressed(InputAction.CallbackContext context)
    {
        if (!isPlayer) return;

        if (context.canceled)
        {
            unitEventHandler.RaiseEvent("OnAbility2Casted", null);
        }
    }

    // 'E' Key
    public void OnThirdAbilityPressed(InputAction.CallbackContext context)
    {
        if (!isPlayer) return;

        if (context.canceled)
        {
            unitEventHandler.RaiseEvent("OnAbility3Casted", null);
        }
    }

    // 'R' Key
    public void OnFourthAbilityPressed(InputAction.CallbackContext context)
    {
        if (!isPlayer) return;

        if (context.canceled)
        {
            unitEventHandler.RaiseEvent("OnAbility4Casted", null);
        }
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
            .DORotate(new Vector3(float.Epsilon, float.Epsilon, AngleToTarget(destination)),
                turnRate * 360)
            .SetSpeedBased().SetEase(Ease.Linear).OnComplete(() => Move(destination));
    }

    private void Move(Vector3 destination)
    {
        var eulerAnglesZ = PseudoObject.transform.rotation.eulerAngles.z;
        agent.speed = movementSpeed * (1 - Mathf.Abs(Mathf.Sin(eulerAnglesZ * Mathf.Deg2Rad)) / 2);
        agent.SetDestination(destination);
        unitEventHandler.RaiseEvent("OnPseudoObjectRotationChanged", eulerAnglesZ);
    }

    private float AngleToTarget(Vector3 target)
    {
        Vector2 vectorToTarget = target - transform.position;

        return Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
    }

    // members used for ability execution
    private AbilityType _currentAbilityType;
    private readonly Dictionary<string, object> _allTargets = new Dictionary<string, object>();

    private void AbilityInput(object target)
    {
        if (_currentAbilityType == AbilityType.TargetPoint)
        {
            _castTargetPosition = (Vector3)target;
            _allTargets["Target Point"] = _castTargetPosition;
        }
        else if (_currentAbilityType == AbilityType.TargetUnit)
        {
            var targetUnit = (Unit)target;
            _castTargetPosition = targetUnit.transform.position;
            _allTargets["Target Unit"] = target;
            _allTargets["Target Unit Position"] = _castTargetPosition;
        }
        else if (_currentAbilityType == AbilityType.TargetArea)
        {
            _castTargetPosition = (Vector3)target;
            _allTargets["Target Center"] = _castTargetPosition;
        }
        else if (_currentAbilityType == AbilityType.NoTarget)
        {
            _castTargetPosition = (Vector3)target;
            _allTargets["Target Center"] = _castTargetPosition;
        }
    }

    private IEnumerator CastAbility(Ability ability)
    {
        Stop();

        _currentAbilityType = ability.inputType;
        _allTargets.Clear();

        // by default, _allTargets should contain a reference to unit and a reference to unit position separately
        _allTargets["Executing Unit"] = this;
        _allTargets["Executing Unit Position"] = transform.position;

        if (_currentAbilityType != AbilityType.NoTarget)
        {
            if (isPlayer)
            {
                var playerAgent = GetComponent<PlayerAgent>();
                yield return StartCoroutine(playerAgent.ProcessTargetInput(ability));
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

        if (Vector3.Distance(transform.position, _castTargetPosition) <= ability.abilityStats["Cast Range"])
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
        while (Vector3.Distance(transform.position, _castTargetPosition) >= ability.abilityStats["Cast Range"])
        {
            yield return new WaitForFixedUpdate();
            agent.SetDestination(_castTargetPosition);
        }

        _pendingCast = null;
        TurnAndExecuteAbility(ability);
    }

    private void TurnAndExecuteAbility(Ability ability)
    {
        Stop();

        PseudoObject.transform
            .DORotate(new Vector3(float.Epsilon, float.Epsilon, AngleToTarget(_castTargetPosition)),
                turnRate * 360)
            .SetSpeedBased().SetEase(Ease.Linear).OnComplete(() => ExecuteAbility(ability));
    }

    private void ExecuteAbility(Ability ability)
    {
        // Update the Player's rotation
        float eulerAnglesZ = PseudoObject.transform.rotation.eulerAngles.z;
        unitEventHandler.RaiseEvent("OnPseudoObjectRotationChanged", eulerAnglesZ);

        // used by AI to indicate ability has started execution
        unitEventHandler.RaiseEvent("OnAbilityStartedExecuting", null);

        foreach (var outcome in ability.outcomes)
        {
            float executionTime;

            if (outcome.Trigger.IsNormalizedTime)
            {
                executionTime = outcome.Trigger.ExecutionTime * ability.duration;
            }
            else
            {
                executionTime = outcome.Trigger.ExecutionTime;
            }

            StartCoroutine(ExecuteOutcome(outcome, ability.abilityStats, executionTime, outcome.Duration));
        }
    }

    private IEnumerator ExecuteOutcome(Outcome outcome, AbilityStatsDict abilityStats, float timeToExecute,
        float duration)
    {
        yield return new WaitForSeconds(timeToExecute);

        foreach (var effect in outcome.Effects)
        {
            effect.ExecuteEffect(abilityStats, _allTargets);
        }

        yield return null;
    }

    private IEnumerator ExecuteConstantUpdateGameAction(GameAction gameAction, object param, float duration)
    {
        // float timeElapsed = 0;

        // while(timeElapsed < duration) {
        //     print(timeElapsed);
        //     timeElapsed += Time.deltaTime;
        //     GameActionBlock.Invoke(param);
        //     yield return null;
        // }

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

            if (_speed < 0.005f)
            {
                if (_currentAnimation != AnimationType.Stop)
                {
                    _currentAnimation = AnimationType.Stop;
                }
                else return;

                _stopAnimationTimer -= Time.deltaTime;

                if (_stopAnimationTimer <= float.Epsilon)
                {
                    unitEventHandler.RaiseEvent("OnStopMoveAnimation", null);
                }
            }
            else
            {
                if (_currentAnimation != AnimationType.Move)
                {
                    _currentAnimation = AnimationType.Move;
                }
                else return;

                _stopAnimationTimer = 1 / turnRate;
                unitEventHandler.RaiseEvent("OnStartMoveAnimation", null);
            }
        }
    }
}