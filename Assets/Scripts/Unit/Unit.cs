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

    private void OnDisable()
    {
        _unitEventHandler.StopListening("OnMoveOrderIssued", TurnAndMove);
        _unitEventHandler.StopListening("OnStopOrderIssued", Stop);
    }

    private void Move(object destination)
    {
        agent.SetDestination((Vector3)destination);
    }

    private void Stop(object arg0)
    {
        agent.SetDestination(transform.position);
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

    public void TurnAndMove(object targetPoint)
    {
        _pseudoObject.transform
            .DORotate(new Vector3(float.Epsilon, float.Epsilon, AngleToDestination((Vector3)targetPoint)),
                turnRate * 360)
            .SetSpeedBased().SetEase(Ease.Linear).OnComplete(() => Move(targetPoint));
    }

    private float AngleToDestination(Vector3 destination)
    {
        Vector2 angle = destination - transform.position;

        return Mathf.Atan2(angle.y, angle.x) * 180 / Mathf.PI;
    }

    public IEnumerator ExecuteAbility(Ability ability, List<List<object>> outcomeParameters)
    {
        Stop(null);
        PlayerAgent playerAgent = GetComponent<PlayerAgent>();

        yield return StartCoroutine(playerAgent.ProcessTargetInput(ability.InputType));

        for (int i = 0; i < ability.Outcomes.Length; i++)
        {
            Outcome outcome = ability.Outcomes[i];
            float executionTime;
            if (outcome.Trigger.IsNormalizedTime)
            {
                executionTime = outcome.Trigger.ExecutionTime * ability.Duration;
            }
            else
            {
                executionTime = outcome.Trigger.ExecutionTime;
            }

            StartCoroutine(ExecuteOutcome(outcome, executionTime, outcome.Duration, outcomeParameters[i]));
        }
    }

    List<List<object>> outcomeParameters = new List<List<object>>();

    void AbilityInputHandler(object param)
    {
        outcomeParameters.Clear();
        Vector3 pointTarget = (Vector3)param;

        List<object> outcome1Param = new List<object>();
        outcome1Param.Add(new DisappearActionData(this)); // construct data struct for disappear gameAction
        outcomeParameters.Add(outcome1Param);

        List<object> outcome2Param = new List<object>();
        outcome2Param.Add(new TeleportActionData(this, pointTarget));
        outcome2Param.Add(new ReappearActionData(this)); // construct data struct for reappear gameAction
        outcomeParameters.Add(outcome2Param);
    }

    IEnumerator ExecuteOutcome(Outcome outcome, float timeToExecute, float duration, object param)
    {
        List<object> outcomeParams = (List<object>)param;

        yield return new WaitForSeconds(timeToExecute);

        for (int i = 0; i < outcome.Effects.Length; i++)
        {
            GameAction gameAction = outcome.Effects[i];

            if (gameAction.gameActionBehaviour == GameActionBehaviour.ConstantUpdate)
            {
                StartCoroutine(ExecuteConstantUpdateGameAction(gameAction, outcomeParams[i], duration));
            }
            else
            {
                gameAction.Invoke(outcomeParams[i]);
            }
        }
    }

    IEnumerator ExecuteConstantUpdateGameAction(GameAction gameAction, object param, float duration)
    {
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            print(timeElapsed);
            timeElapsed += Time.deltaTime;
            gameAction.Invoke(param);
            yield return null;
        }
    }

    void TestBlink()
    {
        StartCoroutine(ExecuteAbility(abilities[0], outcomeParameters)); // testing first skill, "Blink"
    }

    void TestSnipe()
    {
        List<List<object>> param = new List<List<object>>();

        SwarmerAIAgent dummySwarmer = FindObjectOfType<SwarmerAIAgent>();
        List<object> outcome1Param = new List<object>();
        outcome1Param.Add(new RotateToFaceUnitData(this,
            dummySwarmer.transform.position)); // construct data struct for disappear gameAction
        param.Add(outcome1Param);

        StartCoroutine(ExecuteAbility(abilities[1], param)); // testing second skill, "Snipe"
    }
}