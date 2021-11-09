using UnityEngine;

public class PlayerAgent : Agent
{
    [Header("Input")] public float autoClickInterval = 0.1f;
    private float _autoClickTimer;
    [Header("Misc.")] public float positionUpdateInterval = 0.1f;
    private float _positionUpdateTimer;
    private Camera _camera;

    protected override void Awake()
    {
        base.Awake();
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        EventManager.StartListening("OnMoveOrderIssued", OnMoveOrderIssued);
        EventManager.StartListening("OnStopOrderIssued", OnStopOrderIssued);
    }

    private void OnDisable()
    {
        EventManager.StopListening("OnMoveOrderIssued", OnMoveOrderIssued);
        EventManager.StopListening("OnStopOrderIssued", OnStopOrderIssued);
    }

    private void Update()
    {
        UpdatePosition();
        PlayerInput();
    }

    private void UpdatePosition()
    {
        _positionUpdateTimer += Time.deltaTime;

        if (_positionUpdateTimer >= positionUpdateInterval)
        {
            _positionUpdateTimer = float.Epsilon;
            EventManager.RaiseEvent("OnPlayerPositionChanged", transform.position);
        }
    }

    private void PlayerInput()
    {
        // Right mouse button
        if (Input.GetMouseButtonDown(1))
        {
            EventManager.RaiseEvent("OnMoveOrderIssued", CursorWorldPosition());
        }
        else if (Input.GetMouseButton(1))
        {
            // Auto-repeat click when held
            _autoClickTimer += Time.deltaTime;

            if (_autoClickTimer >= autoClickInterval)
            {
                _autoClickTimer = float.Epsilon;
                EventManager.RaiseEvent("OnMoveOrderIssued", CursorWorldPosition());
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            _autoClickTimer = float.Epsilon;
        }

        // 'S' key
        if (Input.GetKeyDown(KeyCode.S))
        {
            EventManager.RaiseEvent("OnStopOrderIssued", null);
        }
    }

    private Vector3 CursorWorldPosition()
    {
        Vector2 screenPosition = Input.mousePosition;
        Vector3 worldPosition = _camera.ScreenToWorldPoint(screenPosition);

        return new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
    }

    private void OnMoveOrderIssued(object destination)
    {
        agent.SetDestination((Vector3)destination);
    }

    private void OnStopOrderIssued(object arg0)
    {
        agent.SetDestination(transform.position);
    }
}