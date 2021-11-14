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
    private EventProcessor _unitEventHandler; // Internal event handler
    [Header("Stats")] public float movementSpeed = 3.5f;
    public float turnRate = 5f;
    [HideInInspector] public bool isPlayer;
    [HideInInspector] public NavMeshAgent agent;
    [Header("Misc.")] public float updateInterval = 0.1f;
    private float _positionUpdateTimer;
    private GameObject _pseudoObject;
    [Header("Abilities")] [SerializeField] private List<Ability> abilities;
    private Vector3 _castTargetPosition;
    private IEnumerator _pendingCast;

    private void Awake()
    {
        _unitEventHandler = GetComponent<UnitEventManager>().UnitEventHandler;
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
        _unitEventHandler.StartListening("OnMoveOrderIssued", TurnAndMove);
        _unitEventHandler.StartListening("OnStopOrderIssued", Stop);

        EventManager.StartListening("OnAbilityInputSet", AbilityInputHandler); // temporary for testing
    }

    private void OnDisable()
    {
        _unitEventHandler.StopListening("OnMoveOrderIssued", TurnAndMove);
        _unitEventHandler.StopListening("OnStopOrderIssued", Stop);
    }

    private void Update()
    {
        UpdatePosition();

        if (gameObject.name == "Character")
        {
            if (Input.GetKeyUp(KeyCode.Q))
            {
                TestBlink();
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                TestSnipe();
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
            }
            else if (Input.GetKeyUp(KeyCode.R))
            {
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

    private void Stop(object @null)
    {
        if (_pendingCast != null) StopCoroutine(_pendingCast);
        _pseudoObject.transform.DOKill();
        agent.SetDestination(transform.position);
    }

    public void TurnAndMove(object destination)
    {
        Stop(null);

        _pseudoObject.transform
            .DORotate(new Vector3(float.Epsilon, float.Epsilon, AngleToTarget((Vector3)destination)),
                turnRate * 360)
            .SetSpeedBased().SetEase(Ease.Linear).OnComplete(() => Move(destination));
    }

    private void Move(object destination)
    {
        agent.SetDestination((Vector3)destination);
    }

    private float AngleToTarget(Vector3 target)
    {
        Vector2 angle = target - transform.position;

        return Mathf.Atan2(angle.y, angle.x) * 180 / Mathf.PI;
    }

    // members used for ability execution
    private AbilityType _currentAbilityType;
    private readonly Dictionary<string, object> _inputTargets = new Dictionary<string, object>();
    private readonly Dictionary<string, List<object>> _effectTargets = new Dictionary<string, List<object>>();

    public IEnumerator CastAbility(Ability ability)
    {
        Stop(null);

        _currentAbilityType = ability.InputType;
        _inputTargets.Clear();
        _effectTargets.Clear();

        PlayerAgent playerAgent = GetComponent<PlayerAgent>();

        Debug.Log("Waiting for ability input");
        yield return StartCoroutine(playerAgent.ProcessTargetInput(_currentAbilityType));

        if (Vector3.Distance(transform.position, _castTargetPosition) <= ability.castRange)
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

    private void AbilityInputHandler(object param)
    {
        _castTargetPosition = (Vector3)param;

        if (_currentAbilityType == AbilityType.TargetPoint)
        {
            _inputTargets["Target Point"] = (Vector3)param;
        }
        else if (_currentAbilityType == AbilityType.TargetUnit)
        {
            _inputTargets["Target Unit"] = (Vector3)param;
        }
        else if (_currentAbilityType == AbilityType.TargetArea)
        {
            _inputTargets["Target Center"] = (Vector3)param;
        }
    }

    private IEnumerator PendingCast(Ability ability)
    {
        while (Vector3.Distance(transform.position, _castTargetPosition) >= ability.castRange)
        {
            yield return new WaitForFixedUpdate();
            agent.SetDestination(_castTargetPosition);
        }

        _pendingCast = null;
        TurnAndExecuteAbility(ability);
    }

    private void TurnAndExecuteAbility(Ability ability)
    {
        Stop(null);

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

            StartCoroutine(ExecuteOutcome(outcome, executionTime, outcome.Duration));
        }
    }

    private IEnumerator ExecuteOutcome(Outcome outcome, float timeToExecute, float duration)
    {
        yield return new WaitForSeconds(timeToExecute);

        foreach (var effect in outcome.Effects)
        {
            effect.ExecuteEffect(this, _inputTargets, _effectTargets);
        }

        yield return null;
    }

    private IEnumerator ExecuteConstantUpdateGameAction(GameAction gameAction, object param, float duration)
    {
        // float timeElapsed = 0;

        // while(timeElapsed < duration) {
        //     print(timeElapsed);
        //     timeElapsed += Time.deltaTime;
        //     gameAction.Invoke(param);
        //     yield return null;
        // }

        yield return null;
    }

    private void TestBlink()
    {
        StartCoroutine(CastAbility(abilities[0])); // testing first skill, "Blink"
    }

    private void TestSnipe()
    {
        List<List<object>> param = new List<List<object>>();

        SwarmerAIAgent dummySwarmer = FindObjectOfType<SwarmerAIAgent>();
        List<object> outcome1Param = new List<object>();
        outcome1Param.Add(new RotateToFaceUnitData(this,
            dummySwarmer.transform.position)); // construct data struct for disappear gameAction
        param.Add(outcome1Param);

        StartCoroutine(CastAbility(abilities[1])); // testing second skill, "Snipe"
    }
}