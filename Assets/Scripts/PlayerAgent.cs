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
            unitEventHandler.RaiseEvent("OnPlayerPositionChanged", transform.position);
        }
    }

    private void PlayerInput()
    {
        // Right mouse button
        if (Input.GetMouseButtonDown(1))
        {
            unitEventHandler.RaiseEvent("OnMoveOrderIssued", CursorWorldPosition());
        }
        else if (Input.GetMouseButton(1))
        {
            // Auto-repeat click when held
            _autoClickTimer += Time.deltaTime;

            if (_autoClickTimer >= autoClickInterval)
            {
                _autoClickTimer = float.Epsilon;
                unitEventHandler.RaiseEvent("OnMoveOrderIssued", CursorWorldPosition());
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            _autoClickTimer = float.Epsilon;
        }

        // 'S' key
        if (Input.GetKeyDown(KeyCode.S))
        {
            unitEventHandler.RaiseEvent("OnStopOrderIssued", null);
        }
    }

    private Vector3 CursorWorldPosition()
    {
        Vector2 screenPosition = Input.mousePosition;
        Vector3 worldPosition = _camera.ScreenToWorldPoint(screenPosition);

        return new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
    }
}