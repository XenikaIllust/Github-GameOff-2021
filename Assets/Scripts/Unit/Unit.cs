using System.Collections;
using System.Collections.Generic;
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
    // Internal event handler
    EventProcessor unitEventHandler;

    [HideInInspector] public bool isPlayer;
    [HideInInspector] public NavMeshAgent agent;
    [Header("Misc.")] public float positionUpdateInterval = 0.1f;
    private float _positionUpdateTimer;

    float pseudoYRotation = 0;

    [SerializeField] List<Ability> abilities;

    private void Awake()
    {
        unitEventHandler = GetComponent<UnitEventManager>().UnitEventHandler;

        isPlayer = GetComponent<PlayerAgent>() != null;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Start()
    {
        unitEventHandler.StartListening("OnMoveOrderIssued", OnMoveOrderIssued);
        unitEventHandler.StartListening("OnStopOrderIssued", OnStopOrderIssued);
    }

    private void Update()
    {
        UpdatePosition();

        if(this.gameObject.name == "Character") {
            if(Input.GetKeyUp(KeyCode.Q)) {
                TestBlink();
            }
            else if(Input.GetKeyUp(KeyCode.W)) {
                TestSnipe();
            }
            else if(Input.GetKeyUp(KeyCode.E)) {

            }
            else if(Input.GetKeyUp(KeyCode.R)) {

            }
        }
    }

    private void OnDisable()
    {
        unitEventHandler.StopListening("OnMoveOrderIssued", OnMoveOrderIssued);
        unitEventHandler.StopListening("OnStopOrderIssued", OnStopOrderIssued);
    }

    private void OnMoveOrderIssued(object destination)
    {
        agent.SetDestination((Vector3)destination);
    }

    private void OnStopOrderIssued(object arg0)
    {
        agent.SetDestination(transform.position);
    }

    private void UpdatePosition()
    {
        if (!isPlayer) return;

        _positionUpdateTimer += Time.deltaTime;

        if (_positionUpdateTimer >= positionUpdateInterval)
        {
            _positionUpdateTimer = float.Epsilon;
            EventManager.RaiseEvent("OnPlayerPositionChanged", transform.position);
        }
    }

    public void ExecuteAbility(Ability ability, List<List<object>> outcomeParameters) 
    {
        for(int i = 0; i < ability.Outcomes.Length; i++) 
        {
            Outcome outcome = ability.Outcomes[i];
            float executionTime;
            if(outcome.Trigger.IsNormalizedTime) 
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

    IEnumerator ExecuteOutcome(Outcome outcome, float timeToExecute, float duration, object param) 
    {
        List<object> outcomeParams = (List<object>) param;

        yield return new WaitForSeconds(timeToExecute);

        for(int i = 0; i < outcome.Effects.Length; i++) {
            GameAction gameAction = outcome.Effects[i];

            if(gameAction.gameActionBehaviour == GameActionBehaviour.ConstantUpdate) {
                StartCoroutine(ExecuteConstantUpdateGameAction(gameAction, outcomeParams[i], duration));
            }
            else {
                gameAction.Invoke(outcomeParams[i]);
            }
        }
    }

    IEnumerator ExecuteConstantUpdateGameAction(GameAction gameAction, object param, float duration) {
        float timeElapsed = 0;

        while(timeElapsed < duration) {
            timeElapsed += Time.deltaTime;
            gameAction.Invoke(param);
        }

        yield return null;
    }

    void TestBlink() {
        List<List<object>> param = new List<List<object>>();

        List<object> outcome1Param = new List<object>(); 
        outcome1Param.Add( new DisappearActionData(this) ); // construct data struct for disappear gameAction
        param.Add(outcome1Param);

        List<object> outcome2Param = new List<object>();
        outcome2Param.Add( new TeleportActionData(this, new Vector2(2,2)) );
        outcome2Param.Add( new ReappearActionData(this) ); // construct data struct for reappear gameAction
        param.Add(outcome2Param);

        ExecuteAbility(abilities[0], param); // testing first skill, "Blink"
    }

    void TestSnipe() {
        List<List<object>> param = new List<List<object>>();

        SwarmerAIAgent dummySwarmer = FindObjectOfType<SwarmerAIAgent>();
        List<object> outcome1Param = new List<object>(); 
        outcome1Param.Add( new RotateToFaceUnitData(this, dummySwarmer.transform.position) ); // construct data struct for disappear gameAction
        param.Add(outcome1Param);

        ExecuteAbility(abilities[1], param); // testing second skill, "Snipe"
    }
}