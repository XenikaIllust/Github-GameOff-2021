using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAgent : Agent
{
    [Header("Input")] public float autoClickInterval = 0.1f;
    [SerializeField] private SpriteRenderer AOECircle;
    private float _autoClickTimer;
    private Camera _camera;
    private bool _defaultControlsEnabled = true;
    private bool _holdingRightClick;
    private bool _turnOnHighlight;
    private Unit currentHighlightedUnit;
    private IEnumerator _currentCoroutine;

    protected override void Awake()
    {
        base.Awake();
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        EventManager.StartListening("OnGamePaused", StopAbilityInput);
    }

    private void OnDisable()
    {
        EventManager.StopListening("OnGamePaused", StopAbilityInput);
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
            StopAbilityInput(null);
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
        StopAbilityInput(null);

        switch (ability.inputType)
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
                if (currentHighlightedUnit != null) currentHighlightedUnit.GetComponentInChildren<UnitVFXManager>().ClearEffects(); // Clear the previous enemy that was hightlighted
                break;

            case AbilityType.TargetArea:
                float radius = ability.abilityStats["AOE Radius"];
                AOECircle.transform.localScale = new Vector3(radius * 3.3f, radius * 0.5f * 3.3f, 1.0f);
                AOECircle.enabled = true;
                _currentCoroutine = AbilityInputType.AOETargetInput(ability, unitEventHandler);
                yield return StartCoroutine(_currentCoroutine);
                AOECircle.enabled = false;
                break;

            case AbilityType.NoTarget:
                Debug.LogError("This case should never be true");
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        _defaultControlsEnabled = true;
    }

    private void StopAbilityInput(object @null)
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _turnOnHighlight = false;
            if (currentHighlightedUnit != null) currentHighlightedUnit.GetComponentInChildren<UnitVFXManager>().ClearEffects(); // Clear the previous enemy that was hightlighted
            AOECircle.enabled = false;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Change cursor back to default
        }
    }

    private void AOECircleFollowCursor()
    {
        AOECircle.transform.position = new Vector3(CursorWorldPosition().x,
                                                   CursorWorldPosition().y,
                                                   0.0f);
    }

    private void HighlightUnitUnderMouseCursor()
    {
        LayerMask enemyMask = LayerMask.GetMask("Enemy");

        // Get target and check that it's valid
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()),
                                             Vector2.zero,
                                             Mathf.Infinity,
                                             enemyMask);

        if (hit.collider != null)
        {
            Unit highlightedUnit = hit.collider.GetComponent<Unit>(); // Get the new enemy to be highlighted

            // If the highlightedUnit isn't the exact same as it was before then...
            if (highlightedUnit != currentHighlightedUnit || highlightedUnit == null || currentHighlightedUnit == null)
            {
                if (currentHighlightedUnit != null) currentHighlightedUnit.GetComponentInChildren<UnitVFXManager>().ClearEffects(); // Clear the previous enemy that was hightlighted
                currentHighlightedUnit = highlightedUnit; // Set the new enemy as the current one
            }

            if (currentHighlightedUnit == null) return;

            // Highlight the currently selected enemy
            if (currentHighlightedUnit.alliance != thisUnit.alliance)
            {
                currentHighlightedUnit.GetComponentInChildren<UnitVFXManager>().HighlightOutline();
            }
        }
    }
}
