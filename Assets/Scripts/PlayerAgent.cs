using System;
using System.Linq;
using System.Collections;
using UnityEngine;

public class PlayerAgent : Agent
{
    [Header("Input")] public float autoClickInterval = 0.1f;
    private float _autoClickTimer;

    private Camera _camera;

    bool defaultControlsEnabled = true;

    [SerializeField] private SpriteRenderer AOECircle;

    private bool turnOnHighlight = false;

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

    // Responsible for aiming abilities, coupled with AbilityInputType.cs
    public Func<bool> targetInput;

    public IEnumerator ProcessTargetInput(AbilityType abilityType)
    {
        Texture2D cursorTexture = (Texture2D)Resources.Load("AbilityCursor");
        Vector2 hotSpot = new Vector2(24, 24); // The offset from the top left of the cursor to use as the target point
        defaultControlsEnabled = false;

        targetInput = null;
        if (abilityType == AbilityType.TargetPoint)
        {
            Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto); // Change cursor to selection cursor
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0)); // Wait until the player presses the Left Click
            targetInput = AbilityInputType.PointTargetInput;
        }
        else if (abilityType == AbilityType.TargetUnit)
        {
            turnOnHighlight = true;
            Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto); // Change cursor to selection cursor
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0)); // Wait until the player presses the Left Click
            targetInput = AbilityInputType.UnitTargetInput;
            turnOnHighlight = false;
        }
        else if (abilityType == AbilityType.TargetArea)
        {
            float radius = 3.0f; // PLACEHOLDER
            AOECircle.transform.localScale = new Vector3(radius * 1.7f, radius * 1.7f, 1.0f);
            AOECircle.enabled = true;
            Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto); // Change cursor to selection cursor
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0)); // Wait until the player presses the Left Click
            targetInput = AbilityInputType.AOETargetInput;
            AOECircle.enabled = false;
        }
        else if (abilityType == AbilityType.NoTarget)
        {
            targetInput = AbilityInputType.NoTargetInput;
        }

        yield return new WaitUntil(() => targetInput());

        defaultControlsEnabled = true;
    }

    private void AOECircleFollowCursor()
    {
        AOECircle.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
            0.0f
        );
    }

    private void HighlightUnitUnderMouseCursor()
    {
        string[] tags = { "Enemy" };
        LayerMask enemyMask = LayerMask.GetMask("Enemy");

        // Get target and check that it's valid
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
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