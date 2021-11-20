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
    public EventProcessor UnitEventHandler; // Internal event handler
    [Header("Stats")] public float movementSpeed = 3.5f;
    public float turnRate = 5f;
    [HideInInspector] public bool isPlayer;
    [HideInInspector] public NavMeshAgent agent;
    [Header("Misc.")] public float updateInterval = 0.1f;
    private float _positionUpdateTimer;
    private GameObject _pseudoObject;
    [Header("Abilities")] [SerializeField] private Ability[] abilities = new Ability[4];
    private Vector3 _castTargetPosition;
    private IEnumerator _pendingCast;
    private object _aiTarget;

    private void Awake()
    {
        UnitEventHandler = GetComponent<UnitEventManager>().UnitEventHandler;
        isPlayer = GetComponent<PlayerAgent>() != null;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = movementSpeed;
        agent.acceleration = float.MaxValue;
        agent.angularSpeed = float.MaxValue;
        agent.autoBraking = true;

        _pseudoObject = new GameObject("PseudoObject")
        {
            transform =
            {
                parent = transform
            }
        };
    }

    private void OnEnable()
    {
        UnitEventHandler.StartListening("OnStopOrderIssued", OnStopOrderIssued);
        UnitEventHandler.StartListening("OnMoveOrderIssued", OnMoveOrderIssued);
        UnitEventHandler.StartListening("OnDied", OnDied);
        UnitEventHandler.StartListening("On1stAbilityCasted", On1stAbilityCasted);
        UnitEventHandler.StartListening("On2ndAbilityCasted", On2ndAbilityCasted);
        UnitEventHandler.StartListening("On3thAbilityCasted", On3rdAbilityCasted);
        UnitEventHandler.StartListening("On4thAbilityCasted", On4thAbilityCasted);

        if (isPlayer) // temporary solution, may want to revise if the AI will use the same input
        {
            EventManager.StartListening("OnAbilityInputSet", OnAbilityInputSet);
        }
    }

    private void OnDisable()
    {
        UnitEventHandler.StopListening("OnStopOrderIssued", OnStopOrderIssued);
        UnitEventHandler.StopListening("OnMoveOrderIssued", OnMoveOrderIssued);
        UnitEventHandler.StopListening("OnDied", OnDied);
        UnitEventHandler.StopListening("On1stAbilityCasted", On1stAbilityCasted);
        UnitEventHandler.StopListening("On2ndAbilityCasted", On2ndAbilityCasted);
        UnitEventHandler.StopListening("On3thAbilityCasted", On3rdAbilityCasted);
        UnitEventHandler.StopListening("On4thAbilityCasted", On4thAbilityCasted);

        if (isPlayer)
        {
            EventManager.StopListening("OnAbilityInputSet", OnAbilityInputSet); // temporary for testing
        }
    }

    private void OnStopOrderIssued(object @null)
    {
        Stop();
    }

    private void OnMoveOrderIssued(object destination)
    {
        TurnAndMove((Vector3)destination);
    }

    private void On1stAbilityCasted(object target)
    {
        StartCoroutine(CastAbility(abilities[0]));
        if (!isPlayer) _aiTarget = target;
    }

    private void On2ndAbilityCasted(object target)
    {
        StartCoroutine(CastAbility(abilities[1]));
        if (!isPlayer) _aiTarget = target;
    }

    private void On3rdAbilityCasted(object target)
    {
        StartCoroutine(CastAbility(abilities[2]));
        if (!isPlayer) _aiTarget = target;
    }

    private void On4thAbilityCasted(object target)
    {
        StartCoroutine(CastAbility(abilities[3]));
        if (!isPlayer) _aiTarget = target;
    }

    private void OnAbilityInputSet(object target)
    {
        AbilityInput(target);
    }

    private void OnDied(object @null)
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        UpdatePosition();
        UpdateAnimationMovement();
    }

    // 'Q' Key
    public void OnFirstAbilityPressed(InputAction.CallbackContext context)
    {
        if (!isPlayer) return;

        if (context.canceled)
        {
            UnitEventHandler.RaiseEvent("On1stAbilityCasted", null);
        }
    }

    // 'W' Key
    public void OnSecondAbilityPressed(InputAction.CallbackContext context)
    {
        if (!isPlayer) return;

        if (context.canceled)
        {
            UnitEventHandler.RaiseEvent("On2ndAbilityCasted", null);
        }
    }

    // 'E' Key
    public void OnThirdAbilityPressed(InputAction.CallbackContext context)
    {
        if (!isPlayer) return;

        if (context.canceled)
        {
            UnitEventHandler.RaiseEvent("On3rdAbilityCasted", null);
        }
    }

    // 'R' Key
    public void OnFourthAbilityPressed(InputAction.CallbackContext context)
    {
        if (!isPlayer) return;

        if (context.canceled)
        {
            UnitEventHandler.RaiseEvent("On4thAbilityCasted", null);
        }
    }

    private void UpdatePosition()
    {
        if (!isPlayer) return;

        _positionUpdateTimer += Time.deltaTime;

        if (_positionUpdateTimer >= updateInterval)
        {
            _positionUpdateTimer = float.Epsilon;
            EventManager.RaiseEvent("OnPlayerPositionChanged", transform.position);
        }
    }

    private void Stop()
    {
        if (_pendingCast != null) StopCoroutine(_pendingCast);
        _pseudoObject.transform.DOKill();
        agent.SetDestination(transform.position);
    }

    private void TurnAndMove(Vector3 destination)
    {
        Stop();

        _pseudoObject.transform
            .DORotate(new Vector3(float.Epsilon, float.Epsilon, AngleToTarget(destination)),
                turnRate * 360)
            .SetSpeedBased().SetEase(Ease.Linear).OnComplete(() => Move(destination));
    }

    private void Move(Vector3 destination)
    {
        var eulerAnglesZ = _pseudoObject.transform.rotation.eulerAngles.z;
        agent.speed = movementSpeed * (1 - Mathf.Abs(Mathf.Sin(eulerAnglesZ * Mathf.Deg2Rad)) / 2);
        agent.SetDestination(destination);
        UnitEventHandler.RaiseEvent("OnPseudoObjectRotationChanged", eulerAnglesZ);
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
            Unit targetUnit = (Unit)target;
            _castTargetPosition = targetUnit.transform.position;
            _allTargets["Target Unit"] = target;
            _allTargets["Target Unit Position"] = targetUnit.transform.position;
        }
        else if (_currentAbilityType == AbilityType.TargetArea)
        {
            _castTargetPosition = (Vector3)target;
            _allTargets["Target Center"] = _castTargetPosition;
        }
    }

    private IEnumerator CastAbility(Ability ability)
    {
        Stop();

        Debug.Log("Currently executing ability " + ability.name);

        _currentAbilityType = ability.InputType;
        _allTargets.Clear();

        // by default, _allTargets should contain a reference to unit and a reference to unit position separately
        _allTargets["Executing Unit"] = this;
        _allTargets["Executing Unit Position"] = transform.position;

        if (isPlayer)
        {
            PlayerAgent playerAgent = GetComponent<PlayerAgent>();
            yield return StartCoroutine(playerAgent.ProcessTargetInput(ability));
        }
        else
        {
            // TODO: Somehow auto aim the ability to _aiTarget
            AbilityInput(_aiTarget);
        }

        if (Vector3.Distance(transform.position, _castTargetPosition) <= ability.AbilityStats["Cast Range"])
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
        while (Vector3.Distance(transform.position, _castTargetPosition) >= ability.AbilityStats["Cast Range"])
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

        _pseudoObject.transform
            .DORotate(new Vector3(float.Epsilon, float.Epsilon, AngleToTarget(_castTargetPosition)),
                turnRate * 360)
            .SetSpeedBased().SetEase(Ease.Linear).OnComplete(() => ExecuteAbility(ability));
    }

    private void ExecuteAbility(Ability ability)
    {
        foreach (var outcome in ability.Outcomes)
        {
            float executionTime;
            if (outcome.Trigger.IsNormalizedTime)
            {
                executionTime = outcome.Trigger.ExecutionTime * ability.Duration;
            }
            else
            {
                executionTime = outcome.Trigger.ExecutionTime;
            }

            StartCoroutine(ExecuteOutcome(outcome, ability.AbilityStats, executionTime, outcome.Duration));
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

    private void TestQ()
    {
        StartCoroutine(CastAbility(abilities[0]));
    }

    private void TestW()
    {
        StartCoroutine(CastAbility(abilities[1]));
    }

    private void TestE()
    {
        StartCoroutine(CastAbility(abilities[2]));
    }

    private void TestR()
    {
        StartCoroutine(CastAbility(abilities[3]));
    }

    // Animation related functionality
    float _speed;
    Vector2 _lastPosition;

    void UpdateAnimationMovement()
    {
        if (gameObject.name == "MainCharacter")
        {
            _speed = Mathf.Lerp(_speed, ((Vector2)transform.position - _lastPosition).magnitude,
                0.3f /*adjust this number in order to make interpolation quicker or slower*/);
            _lastPosition = (Vector2)transform.position;

            if (_speed > 0.005)
            {
                UnitEventHandler.RaiseEvent("OnStartMoveAnimation", null);
            }
            else
            {
                UnitEventHandler.RaiseEvent("OnStopMoveAnimation", null);
            }
        }
    }
}