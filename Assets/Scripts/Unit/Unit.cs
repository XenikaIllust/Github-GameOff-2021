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

    private void Stop(object @null)
    {
        _pseudoObject.transform.DOKill();
        agent.SetDestination(transform.position);
    }

    private void Move(object destination)
    {
        agent.SetDestination((Vector3)destination);
    }

    public void TurnAndMove(object destination)
    {
        Stop(null);

        _pseudoObject.transform
            .DORotate(new Vector3(float.Epsilon, float.Epsilon, AngleToDestination((Vector3)destination)),
                turnRate * 360)
            .SetSpeedBased().SetEase(Ease.Linear).OnComplete(() => Move(destination));
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

    private float AngleToDestination(Vector3 destination)
    {
        Vector2 angle = destination - transform.position;

        return Mathf.Atan2(angle.y, angle.x) * 180 / Mathf.PI;
    }

    // members used for ability execution
    Ability currentAbility = null;
    Dictionary<string, object> AllTargets = new Dictionary<string, object>();

    public IEnumerator ExecuteAbility(Ability ability)
    {
        Stop(null);

        currentAbility = ability;
        AllTargets.Clear();
        AllTargets["Executing Unit"] = this;
        AllTargets["Executing Unit Position"] = this;

        PlayerAgent playerAgent = GetComponent<PlayerAgent>();

        Debug.Log("Waiting for ability input");
        yield return StartCoroutine(playerAgent.ProcessTargetInput(currentAbility.InputType));
        yield return new WaitUntil(() => inputProcessingComplete);
        inputProcessingComplete = false;

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

            StartCoroutine(ExecuteOutcome(outcome, executionTime, outcome.Duration));
        }
    }

    bool inputProcessingComplete = false;
    void AbilityInputHandler(object param) {
        if (currentAbility.InputType == AbilityType.TargetPoint)
		{
			AllTargets["Target Point"] = (Vector3) param;
		}
        else if (currentAbility.InputType == AbilityType.TargetUnit)
		{
			AllTargets["Target Unit"] = (Vector3) param;
		}
		else if (currentAbility.InputType == AbilityType.TargetArea)
		{
			AllTargets["Target Center"] = (Vector3) param;
		}

        inputProcessingComplete = true;
    }

    IEnumerator ExecuteOutcome(Outcome outcome, float timeToExecute, float duration)
    {
        yield return new WaitForSeconds(timeToExecute);

        for(int i = 0; i < outcome.Effects.Length; i++) {
            Effect effect = outcome.Effects[i];
            effect.ExecuteEffect(this, currentAbility.AbilityStats, AllTargets);
        }

        yield return null;
    }

    IEnumerator ExecuteConstantUpdateGameActionBlock(GameActionBlock GameActionBlock, object param, float duration) {
        // float timeElapsed = 0;

        // while(timeElapsed < duration) {
        //     print(timeElapsed);
        //     timeElapsed += Time.deltaTime;
        //     GameActionBlock.Invoke(param);
        //     yield return null;
        // }

        yield return null;
    }

    void TestBlink()
    {
        StartCoroutine( ExecuteAbility(abilities[0]) ); // testing first skill, "Blink"
    }

    void TestSnipe()
    {
        List<List<object>> param = new List<List<object>>();

        SwarmerAIAgent dummySwarmer = FindObjectOfType<SwarmerAIAgent>();
        List<object> outcome1Param = new List<object>();
        outcome1Param.Add(new RotateToFaceUnitData(this,
            dummySwarmer.transform.position)); // construct data struct for disappear GameActionBlock
        param.Add(outcome1Param);

        StartCoroutine( ExecuteAbility(abilities[1]) ); // testing second skill, "Snipe"
    }
}