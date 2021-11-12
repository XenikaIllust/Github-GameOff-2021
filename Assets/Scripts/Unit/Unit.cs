using System;
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

    Vector2 direction = Vector2.zero;

    float pseudoYRotation = 0f;
    public float PseudoYRotation {
        get { return pseudoYRotation; }
        set { pseudoYRotation = value; }
    }

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
        unitEventHandler.StartListening("OnMoveOrderIssued", Move);
        unitEventHandler.StartListening("OnStopOrderIssued", Stop);
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
        unitEventHandler.StopListening("OnMoveOrderIssued", Move);
        unitEventHandler.StopListening("OnStopOrderIssued", Stop);
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
        UpdatePseudoRotation();

        if (!isPlayer) return;

        _positionUpdateTimer += Time.deltaTime;

        if (_positionUpdateTimer >= positionUpdateInterval)
        {
            _positionUpdateTimer = float.Epsilon;
            EventManager.RaiseEvent("OnPlayerPositionChanged", transform.position);
        }
    }

    private void UpdatePseudoRotation() 
    {
        RotateToFacePoint((Vector2) agent.steeringTarget);
    }

    public void RotateToFacePoint(Vector2 targetPoint) {
        float xDiff = targetPoint.x - transform.position.x;
        float yDiff = targetPoint.y - transform.position.y;

        // if the unit has stopped moving, the calculation for bearing will break because xDiff and yDiff becomes 0.
        // return before it breaks pseudoYRotation
        if(Mathf.Round(xDiff * 100) / 100 == 0 && Mathf.Round(yDiff * 100) / 100 == 0) 
        { 
            return; 
        }

        // we take the absolute differences, just to assume they are in the 1st quadrant. We will deal with signage later.
        float absXDiff = Mathf.Abs(xDiff);
        float absYDiff = Mathf.Abs(yDiff);

        // we are calculating the tangent with respect to the y axis. This is so that rotation convention follows compass bearing.
        float angle = Mathf.Atan( absXDiff / absYDiff ) * Mathf.Rad2Deg;

        // now we need to consider the 4 quadrants (+x +y, +x -y, -x -y, -x +y), going clockwise.
        // i know it could have been written in a more shorthand way but I explicitly wanted to show the logic
        if(xDiff >= Mathf.Epsilon && yDiff >= Mathf.Epsilon) {
            pseudoYRotation = angle;
        }
        else if(xDiff >= Mathf.Epsilon && yDiff < Mathf.Epsilon) {
            pseudoYRotation = 180 - angle;
        }
        else if(xDiff < Mathf.Epsilon && yDiff < Mathf.Epsilon) {
            pseudoYRotation = 180 + angle;
        }
        else {
            pseudoYRotation = 360 - angle;
        }
    }

    public void ExecuteAbility(Ability ability, List<List<object>> outcomeParameters) 
    {
        Stop(null);
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
            print(timeElapsed);
            timeElapsed += Time.deltaTime;
            gameAction.Invoke(param);
            yield return null;
        }
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