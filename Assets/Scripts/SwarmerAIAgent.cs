using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmerAIAgent : Agent
{
    [Header("Stats")] public float hostileRange = 5f;
    public float attackRange = 2f;
    public float attackSpeed = 0.5f;

    private bool _isBusy;
    private Vector3 _playerPosition;

    private bool _1stAbilityOnCooldown = false;

    object _aiTarget;

    private void OnEnable()
    {
        EventManager.StartListening("OnPlayerPositionChanged", OnPlayerPositionChanged);
    }

    private void OnDisable()
    {
        EventManager.StopListening("OnPlayerPositionChanged", OnPlayerPositionChanged);
    }

    private void OnPlayerPositionChanged(object newPosition)
    {
        /*---------------------------------------------------------------------------------
        Put InvokeBestAction in an update loop instead. This function is not needed. Even if 
        the player doesn't change position, the enemies should be making their move. If the 
        function is causing performance issues, then run InvokeBestAction in intervals.
        ---------------------------------------------------------------------------------*/

        _playerPosition = (Vector3)newPosition;
        InvokeBestAction();
    }

    private void InvokeBestAction()
    {
        /*-------------------------------------------------------------------------------
        The behavior may seem straightforward enough to do pure utility but you may really
        want to start integrating behavior trees for this one.
        -------------------------------------------------------------------------------*/

        if (_isBusy) return;

        float distanceFromPlayer = Vector3.Distance(transform.position, _playerPosition);

        // Calculate utility value
        float attackPlayerUtility = float.PositiveInfinity * (attackRange - distanceFromPlayer);
        float chasePlayerUtility = hostileRange - distanceFromPlayer;
        float stopUtility = 0;

        // Invoke the highest valued utility Action
        List<(Action, float)> list = new List<(Action, float)>
        {
            (AttackPlayer, attackPlayerUtility),
            (ChasePlayer, chasePlayerUtility),
            (Stop, stopUtility)
        };

        list.Sort((x, y) => y.Item2.CompareTo(x.Item2));
        list[0].Item1.Invoke();
    }

    private void AttackPlayer()
    {
        /*----------------------------------------------------------------------------------------------
        Comments from xenika:

        Here you should check if _1stAbilityOnCooldown is true. If yes, return; if no, proceed.
        Then you should raise "OnAbility1Casted" event. Get the ability's cooldown value and 
        StartCoroutine(CooldownTimer(abilityCooldown)) to sync with the actual ability's
        cooldown.
        -----------------------------------------------------------------------------------------------*/

        _isBusy = true;

        Stop();
        Invoke(nameof(AttackPlayerFinish), attackSpeed);
    }

    private void AttackPlayerFinish()
    {
        _isBusy = false;

        unitEventHandler.RaiseEvent("OnAbility1Casted", _playerPosition);
    }

    private void ChasePlayer()
    {
        unitEventHandler.RaiseEvent("OnMoveOrderIssued", _playerPosition);
    }

    private void Stop()
    {
        unitEventHandler.RaiseEvent("OnMoveOrderIssued", transform.position);
    }

    public override IEnumerator ProcessTargetInput(Ability ability)
    {
        /*-----------------------------------------------------------------
        This function is only for you to throw out the _aiTarget that you 
        have pre-computed earlier.
        -----------------------------------------------------------------*/

        EventManager.RaiseEvent("OnAbilityInputSet", _aiTarget);
        yield return null;
    }

    public IEnumerator CooldownTimer(float time) {
        _1stAbilityOnCooldown = true;
        yield return new WaitForSeconds(time);
        _1stAbilityOnCooldown = false;
    }
}