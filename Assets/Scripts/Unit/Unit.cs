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
    AbilityType currentAbilityType;
    Dictionary<string, object> InputTargets = new Dictionary<string, object>();
    Dictionary<string, List<object>> EffectTargets = new Dictionary<string, List<object>>();

    public IEnumerator ExecuteAbility(Ability ability)
    {
        Stop(null);

        currentAbilityType = ability.InputType;
        InputTargets.Clear();
        EffectTargets.Clear();

        PlayerAgent playerAgent = GetComponent<PlayerAgent>();

        Debug.Log("Waiting for ability input");
        yield return StartCoroutine(playerAgent.ProcessTargetInput(currentAbilityType));

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

    void AbilityInputHandler(object param) {
        if (currentAbilityType == AbilityType.TargetPoint)
		{
			InputTargets["Target Point"] = (Vector3) param;
		}
        else if (currentAbilityType == AbilityType.TargetUnit)
		{
			InputTargets["Target Unit"] = (Vector3) param;
		}
		else if (currentAbilityType == AbilityType.TargetArea)
		{
			InputTargets["Target Center"] = (Vector3) param;
		}
    }

    IEnumerator ExecuteOutcome(Outcome outcome, float timeToExecute, float duration)
    {
        yield return new WaitForSeconds(timeToExecute);

        for(int i = 0; i < outcome.Effects.Length; i++) {
            Effect effect = outcome.Effects[i];
            effect.ExecuteEffect(this, InputTargets, EffectTargets);
        }

        yield return null;
    }

    IEnumerator ExecuteConstantUpdateGameAction(GameAction gameAction, object param, float duration) {
        // float timeElapsed = 0;

        // while(timeElapsed < duration) {
        //     print(timeElapsed);
        //     timeElapsed += Time.deltaTime;
        //     gameAction.Invoke(param);
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
            dummySwarmer.transform.position)); // construct data struct for disappear gameAction
        param.Add(outcome1Param);

        StartCoroutine( ExecuteAbility(abilities[1]) ); // testing second skill, "Snipe"
    }
}