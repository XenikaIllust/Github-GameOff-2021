using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCharacterUnit : Unit
{
    [Header("Player Character Only")]
    public Ability dashAbility;
    float dashAbilityCooldown;

    public override void OnEnable() {
        base.OnEnable();

        unitEventHandler.StartListening("OnDashAbilityCasted", OnDashAbilityCasted);
    }

    public override void OnDisable()
    {
        base.OnDisable();

        unitEventHandler.StopListening("OnDashAbilityCasted", OnDashAbilityCasted);
    }

    protected override void UpdateTimers()
    {
        base.UpdateTimers();
        dashAbilityCooldown -= Time.deltaTime;
    }

    private void OnDashAbilityCasted(object target)
    {
        if (inputLockDuration > float.Epsilon) return;
        DashAbilityCasted(target);
    }

    public void OnDashAbilityPressed(InputAction.CallbackContext context)
    {
        if (!isPlayer || _isGamePaused) return;

        if (context.started)
        {
            unitEventHandler.RaiseEvent("OnDashAbilityCasted", null);
        }
    }

    private void DashAbilityCasted(object target)
    {
        if (dashAbilityCooldown <= float.Epsilon)
        {
            CastDashAbility();
        }
    }

    private void CastDashAbility()
    {
        _allTargets.Clear();

        // by default, _allTargets should contain a reference to unit and a reference to unit position separately
        _allTargets["Executing Unit"] = this;
        _allTargets["Executing Unit Position"] = transform.position;

        // hard enter the direction in front of the player * distance
        float eulerAnglesZ = PseudoObject.transform.rotation.eulerAngles.z;
        float targetX = transform.position.x + dashAbility.abilityStats["Dash Distance"] * Mathf.Cos(eulerAnglesZ * Mathf.Deg2Rad);
        float targetY = transform.position.y + dashAbility.abilityStats["Dash Distance"] * Mathf.Sin(eulerAnglesZ * Mathf.Deg2Rad);
        Vector3 targetPoint = new Vector3(targetX, targetY, transform.position.z);
        AbilityInput(targetPoint);

        Debug.DrawLine(transform.position, targetPoint, Color.red, 5);
        Debug.Log("origin: " + transform.position + ", direction: " + eulerAnglesZ + " , target point: " + targetPoint);

        StartCoroutine(ExecuteDashAbility());
    }

    private IEnumerator ExecuteDashAbility()
    {
        // Put the dash ability on cooldown
        dashAbilityCooldown = dashAbility.cooldown;

        // Update the Player's rotation
        float eulerAnglesZ = PseudoObject.transform.rotation.eulerAngles.z;
        unitEventHandler.RaiseEvent("OnPseudoObjectRotationChanged", eulerAnglesZ);

        // used by AI to indicate ability has started execution
        unitEventHandler.RaiseEvent("OnAbilityStartedExecuting", null);

        foreach (var outcome in dashAbility.outcomes)
        {
            var executionTime = outcome.Trigger.IsNormalizedTime
                ? outcome.Trigger.ExecutionTime * dashAbility.duration
                : outcome.Trigger.ExecutionTime;

            StartCoroutine(ExecuteOutcome(outcome, dashAbility, executionTime));
        }

        yield return null;
    }
}


