using System;
using System.Collections.Generic;
using UnityEngine;

public class SniperAIAgent : AIAgent
{
    [Header("Stats")] public float hostileRange = 5f;
    public float aimRange = 3f;
    [SerializeField] private Ability aimAbility;
    private float _aimCooldown = float.Epsilon;
    private Vector3 _playerPosition;

    private void Update()
    {
        _aimCooldown -= Time.deltaTime;
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
        float distanceFromPlayer = Vector3.Distance(transform.position, _playerPosition);

        // Calculate utility value
        float aimUtility;
        if (_aimCooldown <= float.Epsilon)
        {
            _aimCooldown = aimAbility.cooldown;
            aimUtility = float.PositiveInfinity * (aimRange - distanceFromPlayer);
        }
        else
        {
            aimUtility = float.NegativeInfinity;
        }

        float chaseUtility = hostileRange - distanceFromPlayer;
        float stopUtility = 0;

        // Invoke the highest valued utility Action
        List<(Action, float)> list = new List<(Action, float)>
        {
            (Aim, aimUtility),
            (Chase, chaseUtility),
            (Stop, stopUtility)
        };

        list.Sort((x, y) => y.Item2.CompareTo(x.Item2));
        list[0].Item1.Invoke();
    }

    private void Aim()
    {
        unitEventHandler.RaiseEvent("OnAbility1Casted", _playerPosition);
    }

    private void Chase()
    {
        unitEventHandler.RaiseEvent("OnMoveOrderIssued", _playerPosition);
    }

    private void Stop()
    {
        unitEventHandler.RaiseEvent("OnMoveOrderIssued", transform.position);
    }
}