using System;
using System.Collections.Generic;
using UnityEngine;

public class SwarmerAIAgent : Agent
{
    private Vector3 _playerPosition;

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
        // Calculate utility value
        float chasePlayerUtility = 2 - Vector3.Distance(transform.position, _playerPosition);
        float stopUtility = 0;

        // Invoke the highest valued utility Action
        List<(Action, float)> list = new List<(Action, float)>(0)
        {
            (ChasePlayer, chasePlayerUtility),
            (Stop, stopUtility)
        };

        list.Sort((x, y) => y.Item2.CompareTo(x.Item2));
        list[0].Item1.Invoke();
    }

    private void ChasePlayer()
    {
        agent.SetDestination(_playerPosition);
    }

    private void Stop()
    {
        agent.SetDestination(transform.position);
    }
}