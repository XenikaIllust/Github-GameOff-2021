using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CastAbility : Action
{
    public int abilityIndex;
    public SharedBool cooldownActive;
    public SharedBool abilityInUse;

    public float coolDown;
    public float abilityExecutionTime;

    EventProcessor unitEventHandler;
    Unit self;

    public override void OnAwake()
    {
        // cache the player
        unitEventHandler = GetComponent<UnitEventManager>().UnitEventHandler;
        self = GetComponent<Unit>();
        cooldownActive.Value = false;
    }

    public override TaskStatus OnUpdate()
    {
        // unitEventHandler.RaiseEvent("On1stAbilityCasted", null);
        Debug.Log(abilityIndex + " ability casted!");
        StartCoroutine(StartAbilityInUseTimer(abilityExecutionTime));
        StartCoroutine(StartCooldown(coolDown));
        return TaskStatus.Success;
    }

    private IEnumerator StartAbilityInUseTimer(float time) {
        /*---------------------------------------------------------------------------
        This function will block execution of other abilities until the current 
        ability is considered finished. The value executionAbilityTime should be
        determined from the Ability Scriptable Object, when the ability has finished
        it's true execution.

        Note: True execution time != Ability duration.
        Ability duration factors in after effects from main ability execution.
        Eg. Astral step deals additional damage after. But main control should resume
        after the teleport and first damage.
        ---------------------------------------------------------------------------*/
        abilityInUse.Value = true;
        yield return new WaitForSeconds(time);
        abilityInUse.Value = false;
    }

    private IEnumerator StartCooldown(float time) { // function for testing
        cooldownActive.Value = true;
        yield return new WaitForSeconds(time);
        cooldownActive.Value = false;
    }
}
