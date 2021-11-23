using System;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAgent : Agent
{
    [Header("Input")] public float autoClickInterval = 0.1f;
    private float _autoClickTimer;

    private Camera _camera;

    bool defaultControlsEnabled = true;

    private bool holdingRightClick = false;

    [SerializeField] private SpriteRenderer AOECircle;

    private bool turnOnHighlight = false;

    private IEnumerator currentCoroutine = null;

    protected override void Awake()
    {
        base.Awake();
        _camera = Camera.main;
    }

    private void Update()
    {
        if (defaultControlsEnabled)
        {
            PlayerInput();
        }

        AOECircleFollowCursor();
        if (turnOnHighlight) HighlightUnitUnderMouseCursor();
    }

    private void PlayerInput()
    {
        if (holdingRightClick)
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
        if (!defaultControlsEnabled)
        {
            StopAbilityInput();
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Change cursor back to default
            defaultControlsEnabled = true;
            return;
        }

        if (context.started)
        {
            unitEventHandler.RaiseEvent("OnMoveOrderIssued", CursorWorldPosition());
        }

        if (context.performed)
        {
            holdingRightClick = true;
        }

        if (context.canceled)
        {
            holdingRightClick = false;
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
    public override IEnumerator ProcessTargetInput(Ability ability)
    {
        defaultControlsEnabled = false;
        StopAbilityInput();

        if (ability.InputType == AbilityType.TargetPoint)
        {
            currentCoroutine = AbilityInputType.PointTargetInput(ability, unitEventHandler);
            yield return StartCoroutine(currentCoroutine);
        }
        else if (ability.InputType == AbilityType.TargetUnit)
        {
            turnOnHighlight = true;
            currentCoroutine = AbilityInputType.UnitTargetInput(ability, unitEventHandler);
            yield return StartCoroutine(currentCoroutine);
            turnOnHighlight = false;
        }
        else if (ability.InputType == AbilityType.TargetArea)
        {
            float radius = ability.AbilityStats["AOE Radius"];
            AOECircle.transform.localScale = new Vector3(radius * 3.3f, radius * 0.5f * 3.3f, 1.0f);
            AOECircle.enabled = true;
            currentCoroutine = AbilityInputType.AOETargetInput(ability, unitEventHandler);
            yield return StartCoroutine(currentCoroutine);
            AOECircle.enabled = false;
        }
        // else if (ability.InputType == AbilityType.NoTarget) yield return StartCoroutine(AbilityInputType.PointTargetInput(ability));

        defaultControlsEnabled = true;
    }

    public void StopAbilityInput()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            turnOnHighlight = false;
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
        string[] tags = { "Enemy" };
        LayerMask enemyMask = LayerMask.GetMask("Enemy");

        // Get target and check that it's valid
        RaycastHit2D hit = Physics2D.Raycast(CursorWorldPosition(),
            direction: Vector2.zero, distance: Mathf.Infinity, layerMask: enemyMask);
        if (hit.collider != null)
        {
            Transform selection = hit.transform;
            if (tags.Contains(selection.tag)) // Check if its the target we want.
            {
                Unit selectedUnit = hit.collider.GetComponent<Unit>();
                Debug.Log(selection.gameObject.name);

                selectedUnit.GetComponentInChildren<UnitVFXManager>().HighlightOutline();
            }
        }
    }
}
