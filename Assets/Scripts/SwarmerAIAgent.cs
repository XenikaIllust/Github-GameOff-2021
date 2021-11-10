using System;
using System.Collections.Generic;
using UnityEngine;

public class SwarmerAIAgent : Agent
{
    [Header("Stats")] public float hostileRange = 5f;
    public float attackRange = 1f;
    public float attackBufferRange = 2f;
    public int attackDamage = 1;
    public float attackSpeed = 0.5f;

    private bool _isBusy;
    private Vector3 _playerPosition;
    private GameObject _playerGameObject;

    protected override void Awake()
    {
        base.Awake();
        _playerGameObject = FindObjectOfType<PlayerAgent>().gameObject;
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
        InvokeBestAction();
    }

    private void InvokeBestAction()
    {
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
        // Start attack animation
        _isBusy = true;

        Stop();
        Invoke(nameof(AttackPlayerFinish), attackSpeed);
    }

    private void AttackPlayerFinish()
    {
        // Finish attack animation
        _isBusy = false;

        if (Vector3.Distance(_playerGameObject.transform.position, transform.position) <= attackBufferRange)
        {
            EventManager.RaiseEvent("OnPlayerAttacked", attackDamage);
            Debug.Log($"Player received {attackDamage} damage");
        }
    }

    private void ChasePlayer()
    {
        unitEventHandler.RaiseEvent("OnMoveOrderIssued", _playerPosition);
    }

    private void Stop()
    {
        unitEventHandler.RaiseEvent("OnMoveOrderIssued", transform.position);
    }
}