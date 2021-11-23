using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAgent : Agent
{
    [Header("Input")] public float autoClickInterval = 0.1f;
    private float _autoClickTimer;
    private Camera _camera;
    private bool _defaultControlsEnabled = true;
    private bool _holdingRightClick;
    [SerializeField] private SpriteRenderer AOECircle;
    private bool _turnOnHighlight;
    private IEnumerator _currentCoroutine;

    protected override void Awake()
    {
        base.Awake();
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_defaultControlsEnabled)
        {
            PlayerInput();
        }

        AOECircleFollowCursor();
        if (_turnOnHighlight) HighlightUnitUnderMouseCursor();
    }

    private void PlayerInput()
    {
        if (_holdingRightClick)
        {
            // Auto-repeat click when held
            _autoClickTimer += Time.deltaTime;

            if (_autoClickTimer >= autoClickInterval)
            {
                _autoClickTimer = float.Epsilon;
                unitEventHandler.RaiseEvent("OnMoveOrderIssued", CursorWorldPosition());
            }
        }
    }

    // Left Click
    public void OnTargetPressed(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            AbilityInputType.hasPressedLeftClick = true;
        }
    }

    // Right Click
    public void OnMovePressed(InputAction.CallbackContext context)
    {
        if (!_defaultControlsEnabled)
        {
            StopAbilityInput();
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Change cursor back to default
            _defaultControlsEnabled = true;
            return;
        }

        if (context.started)
        {
            unitEventHandler.RaiseEvent("OnMoveOrderIssued", CursorWorldPosition());
        }

        if (context.performed)
        {
            _holdingRightClick = true;
        }

        if (context.canceled)
        {
            _holdingRightClick = false;
            _autoClickTimer = float.Epsilon;
        }
    }

    // 'S' Key
    public void OnStopPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            unitEventHandler.RaiseEvent("OnStopOrderIssued", null);
        }
    }

    private Vector3 CursorWorldPosition()
    {
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = _camera.ScreenToWorldPoint(screenPosition);

        return new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
    }

    // Responsible for aiming abilities, coupled with AbilityInputType.cs
    public IEnumerator ProcessTargetInput(Ability ability)
    {
        _defaultControlsEnabled = false;
        StopAbilityInput();

        switch (ability.InputType)
        {
            case AbilityType.TargetPoint:
                _currentCoroutine = AbilityInputType.PointTargetInput(ability, unitEventHandler);
                yield return StartCoroutine(_currentCoroutine);
                break;
            case AbilityType.TargetUnit:
                _turnOnHighlight = true;
                _currentCoroutine = AbilityInputType.UnitTargetInput(ability, unitEventHandler);
                yield return StartCoroutine(_currentCoroutine);
                _turnOnHighlight = false;
                break;
            case AbilityType.TargetArea:
            {
                float radius = ability.AbilityStats["AOE Radius"];
                AOECircle.transform.localScale = new Vector3(radius * 3.3f, radius * 0.5f * 3.3f, 1.0f);
                AOECircle.enabled = true;
                _currentCoroutine = AbilityInputType.AOETargetInput(ability, unitEventHandler);
                yield return StartCoroutine(_currentCoroutine);
                AOECircle.enabled = false;
                break;
            }
            case AbilityType.NoTarget:
                Debug.LogError("this should never happen");
                break;
            default:
                Debug.LogError("this should never happen");
                break;
        }

        _defaultControlsEnabled = true;
    }

    private void StopAbilityInput()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _turnOnHighlight = false;
            AOECircle.enabled = false;
        }
    }

    private void AOECircleFollowCursor()
    {
        AOECircle.transform.position = new Vector3(CursorWorldPosition().x,
            CursorWorldPosition().y,
            0.0f
        );
    }

    private void HighlightUnitUnderMouseCursor()
    {
        LayerMask enemyMask = LayerMask.GetMask("Enemy");

        // Get target and check that it's valid
        RaycastHit2D hit = Physics2D.Raycast(CursorWorldPosition(),
            Vector2.zero, Mathf.Infinity, enemyMask);

        if (hit.collider != null)
        {
            Unit selectedUnit = hit.collider.GetComponent<Unit>();

            if (selectedUnit.allianceId != thisUnit.allianceId)
            {
                selectedUnit.GetComponentInChildren<UnitVFXManager>().HighlightOutline();
            }
        }
    }
}