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
    // Event processor for handling internal messages
    EventProcessor unitEventHandler;

    [HideInInspector] public bool isPlayer;
    [HideInInspector] public NavMeshAgent agent;
    [Header("Misc.")] public float positionUpdateInterval = 0.1f;
    private float _positionUpdateTimer;

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

        if(Input.GetKeyUp(KeyCode.Q)) {
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

    public void ExecuteAbility(Ability ability, List<List<object>> outcomeParameters) {
        for(int i = 0; i < ability.Outcomes.Length; i++) {
            Outcome outcome = ability.Outcomes[i];
            float executionTime;
            if(outcome.Trigger.IsNormalizedTime) {
                executionTime = outcome.Trigger.ExecutionTime * ability.Duration;
            }
            else {
                executionTime = outcome.Trigger.ExecutionTime;
            }

            StartCoroutine(ExecuteOutcome(outcome, executionTime, outcomeParameters[i]));
        }
    }

    IEnumerator ExecuteOutcome(Outcome outcome, float timeToExecute, object param) {
        List<object> outcomeParams = (List<object>) param;

        yield return new WaitForSeconds(timeToExecute);

        for(int i = 0; i < outcome.Effects.Length; i++) {
            GameAction gameAction = outcome.Effects[i];
            gameAction.Invoke(outcomeParams[i]);
        }
    }
}