using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

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

        if (GetComponent<PlayerAgent>()) // temporary solution, may want to revise if the AI will use the same input
        {
            EventManager.StartListening("OnAbilityInputSet", OnAbilityInputSet);
        }
    }

    private void OnDisable()
    {
        UnitEventHandler.StopListening("OnStopOrderIssued", OnStopOrderIssued);
        UnitEventHandler.StopListening("OnMoveOrderIssued", OnMoveOrderIssued);
        UnitEventHandler.StopListening("Ondied", OnDied);

        EventManager.StopListening("OnAbilityInputSet", OnAbilityInputSet); // temporary for testing
    }

    private void OnStopOrderIssued(object @null)
    {
        Stop();
    }

    private void OnMoveOrderIssued(object destination)
    {
        TurnAndMove((Vector3)destination);
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

        // input testing code
        if (gameObject.name == "Character")
        {
            if (Input.GetKeyUp(KeyCode.Q))
            {
                TestQ();
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                TestW();
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                TestE();
            }
            else if (Input.GetKeyUp(KeyCode.R))
            {
                TestR();
            }
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

        UnitEventHandler.RaiseEvent("OnPseudoObjectRotationChanged", _pseudoObject.transform.rotation.eulerAngles.z);
    }

    private void Move(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    private float AngleToTarget(Vector3 target)
    {
        Vector2 angle = target - transform.position;

        return Mathf.Atan2(angle.y, angle.x) * 180 / Mathf.PI;
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

        PlayerAgent playerAgent = GetComponent<PlayerAgent>();

        yield return StartCoroutine(playerAgent.ProcessTargetInput(ability));

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
}
