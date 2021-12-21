using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySilencerUnit : Unit
{
    [Header("Enemy Silencer Only")]
    int stolenAbilityIndex;
    MainCharacterUnit mainCharacterUnit;

    MageAI AI;

    protected override void Awake() {
        base.Awake();

        mainCharacterUnit = FindObjectOfType<MainCharacterUnit>();
        AI = GetComponent<MageAI>();
    }

    protected override void Update() {
        base.Update();

        if(_allTargets.ContainsKey("StolenAbilityIndex")) {
            stolenAbilityIndex = (int) _allTargets["StolenAbilityIndex"];
            AI.abilityTaken = true;
        }
    }

    protected override void OnDeath(object @null)
    {
        // "if a skill is stolen, return the skill to the player"
        
        if(AI.abilityTaken) {
            mainCharacterUnit.abilitySilenceDurationList[stolenAbilityIndex - 1] = 0;
        }

        base.OnDeath(@null);
    }
}
