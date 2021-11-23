using System;
using System.Collections.Generic;
using UnityEngine;

public class SwarmerAIAgent : Agent
{
    [Header("Stats")] public float hostileRange = 5f;
    public float attackRange = 2f;
    [SerializeField] private Ability attackAbility;
    private float _attackCooldown = float.Epsilon;
    private Vector3 _playerPosition;

    private void Update()
    {
        InvokeBestAction();
    }

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
        _playerPosition = (Vector3)newPosition;
    }

    private void InvokeBestAction()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, _playerPosition);

        // Calculate utility value
        _attackCooldown -= Time.deltaTime;
        float attackPlayerUtility;
        if (_attackCooldown <= float.Epsilon)
        {
            _attackCooldown = attackAbility.Cooldown;
            attackPlayerUtility = float.PositiveInfinity * (attackRange - distanceFromPlayer);
        }
        else
        {
            attackPlayerUtility = float.NegativeInfinity;
        }

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

    // public IEnumerator CooldownTimer(float time)
    // {
    //     _1stAbilityOnCooldown = true;
    //     yield return new WaitForSeconds(time);
    //     _1stAbilityOnCooldown = false;
    // }
}