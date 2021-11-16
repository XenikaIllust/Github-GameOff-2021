using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAgent : Agent
{
    [Header("Input")] public float autoClickInterval = 0.1f;
    private float _autoClickTimer;

    private Camera _camera;

    bool defaultControlsEnabled = true;

	[SerializeField] private SpriteRenderer AOECircle;

    protected override void Awake()
    {
        base.Awake();
        _camera = Camera.main;
    }

    private void Update()
    {
        if(defaultControlsEnabled) {
            PlayerInput();
        }
		AOECircleFollowCursor();
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
        Debug.Log("ProcessTargetInput is executed!");
		Texture2D cursorTexture = (Texture2D) Resources.Load("AbilityCursor");
		defaultControlsEnabled = false;

        targetInput = null;
		if (abilityType == AbilityType.TargetPoint)
		{
			Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto); // Change cursor to selection cursor
			yield return new WaitUntil(() => Input.GetMouseButtonUp(0)); // Wait until the player presses the Left Click
			targetInput = AbilityInputType.PointTargetInput;
		}
		else if (abilityType == AbilityType.TargetUnit)
		{
			Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto); // Change cursor to selection cursor
			yield return new WaitUntil(() => Input.GetMouseButtonUp(0)); // Wait until the player presses the Left Click
			targetInput = AbilityInputType.UnitTargetInput;
		}
		else if (abilityType == AbilityType.TargetArea)
		{
			float radius = 3.0f; // PLACEHOLDER
			AOECircle.transform.localScale = new Vector3(radius * 1.7f, radius * 1.7f, 1.0f);
			AOECircle.enabled = true;
			Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto); // Change cursor to selection cursor
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
}