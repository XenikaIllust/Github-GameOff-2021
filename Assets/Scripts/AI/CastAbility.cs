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

    float abilityRange;
    public float abilityExecutionTime;

    EventProcessor unitEventHandler;
    Unit self;
    Unit playerUnit;

    /*----------------------------------------------------------------------
    The variables below are used for utility calculations for the AI to 
    determine the best skill to use at any moment.
    
    Below are the weights for determining how important a characteristic is
    to an ability, by weight, based on these 4 characteristics:

    1. Enemy within range of ability
    2. Directional difference from current orientation to face enemy direction
    3. Ability damage
    4. Ability cooldown
    ----------------------------------------------------------------------*/

    public float w1; // weightage of importance for enemy being within range of ability
    public float w2; // weightage of importance for directional difference to enemy
    public float w3; // weightage of importance for ability damage
    public float w4; // weightage of importance for ability cooldown

    /*----------------------------------------------------------------------
    Below are the normalized scores for the above 4 stats, either calculated
    or manually entered as necessary:
    ----------------------------------------------------------------------*/

    float n1; // normalized score for computation of enemy distance to ability range
    float n2; // normalized score for computation of directional difference from current orientation to face enemy direction
    public float n3; // normalized score for ranking of damage (0.25, 0.5, 0.75, 1 because of 4 skills). Manually entered
    public float n4; // normalized score for ranking of cooldown (0.25, 0.5, 0.75, 1 because of 4 skills). Manually entered

    public override void OnAwake()
    {
        unitEventHandler = GetComponent<UnitEventManager>().UnitEventHandler;
        self = GetComponent<Unit>();
        playerUnit = GameObject.FindObjectOfType<PlayerAgent>().GetComponent<Unit>();
        cooldownActive.Value = false;
        abilityInUse.Value = false;

        abilityRange = self.abilities[abilityIndex].AbilityStats["Cast Range"];
    }

    public override TaskStatus OnUpdate()
    {
        unitEventHandler.RaiseEvent("On1stAbilityCasted", null);
        Debug.Log(abilityIndex + " ability casted!");
        StartCoroutine(StartAbilityInUseTimer(self.abilities[abilityIndex].Duration));
        StartCoroutine(StartCooldown(self.abilities[abilityIndex].Cooldown));
        return TaskStatus.Success;
    }

    public override float GetUtility()
    {
        n1 = GetNormalizedDistanceScore();
        n2 = GetNormalizedDirectionDifferenceScore();

        float utility = n1 * w1 + n2 * w2 + n3 * w3 + n4 * w4;

        Debug.Log(self.abilities[abilityIndex].name + " " + utility);

        return utility;
    }

    private float GetNormalizedDistanceScore() {
        float normalizedDistanceScore = 0;

        float distance = (playerUnit.transform.position - transform.position).magnitude;
        if(distance <= abilityRange) {
            normalizedDistanceScore = 1;
        }
        else {
            normalizedDistanceScore = 1 / distance;
        }

        return normalizedDistanceScore;
    }

    private float GetNormalizedDirectionDifferenceScore() {
        float selfDirectionBearing = MathUtils.ConvertStandardToAzimuth(self.PseudoObject.transform.rotation.eulerAngles.z);

        float selfToPlayerAngle = Vector2.Angle(Vector2.zero, (self.transform.position - playerUnit.transform.position));
        float angleDifference = Mathf.Abs(selfDirectionBearing - selfToPlayerAngle);
        float smallestAngle = Mathf.Min(angleDifference, 360 - angleDifference);

        float normalizedDirectionDifferenceScore = (smallestAngle - 180) / -180;

        return normalizedDirectionDifferenceScore;
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
