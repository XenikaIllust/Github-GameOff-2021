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

	// PLACEHOLDER CODE FOR TESTING AbilityInputType.cs
	public Func<bool> targetInput;
    bool targetInputCompleted = false;
	public IEnumerator ProcessTargetInput(AbilityType abilityType)
	{
        Debug.Log("ProcessTargetInput is executed!");
        defaultControlsEnabled = false;

        targetInput = null;

		if (abilityType == AbilityType.TargetPoint)
		{
			yield return new WaitUntil(() => Input.GetMouseButtonUp(0)); // Wait until the player presses the Left Click
			targetInput = AbilityInputType.PointTargetInput;
		}
		else if (abilityType == AbilityType.TargetUnit)
		{
			yield return new WaitUntil(() => Input.GetMouseButtonUp(0)); // Wait until the player presses the Left Click
			targetInput = AbilityInputType.UnitTargetInput;
		}
		else if (abilityType == AbilityType.TargetArea)
		{
			yield return new WaitUntil(() => Input.GetMouseButtonUp(0)); // Wait until the player presses the Left Click
			targetInput = AbilityInputType.AOETargetInput;
		}
		else if (abilityType == AbilityType.NoTarget)
		{
			targetInput = AbilityInputType.NoTargetInput;
		}
        else {
            targetInput = null;
        }

        targetInputCompleted = targetInput();
		// yield return new WaitUntil(() => targetInputCompleted == true);
        targetInputCompleted = false;

        defaultControlsEnabled = true;
	}
}