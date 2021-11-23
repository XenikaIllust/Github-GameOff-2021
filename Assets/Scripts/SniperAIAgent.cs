using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SniperAIAgent : Agent
{
    [SerializeField] private GameObject compass;

    [Header("Stats")] public float hostileRange = 5f;
    public float aimRange = 3f;
    public float aimTurnRate = 1f;
    public float aimTime = 5f;

    private bool _isAiming;
    private bool _isShooting; // TODO: delete this line later, only for testing visual
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
        if (_isAiming)
        {
            if (_isShooting) return; // TODO: delete this line later, only for testing visual
            Turn();
        }
        else
        {
            if (_isShooting) return; // TODO: delete this line later, only for testing visual
            float distanceFromPlayer = Vector3.Distance(transform.position, _playerPosition);

            // Calculate utility value
            float aimUtility = float.PositiveInfinity * (aimRange - distanceFromPlayer);
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
    }

    private void Aim()
    {
        _isAiming = true;

        EventManager.RaiseEvent("OnPlaySound", "SniperTargeting");

        Stop();

        // Snap to target
        compass.transform
            .DORotate(new Vector3(float.Epsilon, float.Epsilon, AngleToDestination(_playerPosition)),
                float.PositiveInfinity)
            .SetSpeedBased().SetEase(Ease.Linear);

        compass.SetActive(true);

        Invoke(nameof(Shot), aimTime);
    }

    private void Turn()
    {
        compass.transform
            .DORotate(new Vector3(float.Epsilon, float.Epsilon, AngleToDestination(_playerPosition)),
                aimTurnRate * 360)
            .SetSpeedBased().SetEase(Ease.Linear);
    }

    private void Shot()
    {
        compass.transform.DOKill();
        compass.transform.DOScaleX(100f, float.Epsilon).SetEase(Ease.Linear);

        _isShooting = true; // TODO: delete this line later, only for testing visual
        EventManager.RaiseEvent("OnPlaySound", "SniperFired");
        Invoke(nameof(ResetAim), 0.5f);
    }

    private void ResetAim()
    {
        compass.SetActive(false);
        compass.transform.DOScaleX(5f, float.Epsilon);

        _isShooting = false; // TODO: delete this line later, only for testing visual
        _isAiming = false;
        EventManager.RaiseEvent("OnPlaySound", "SniperLoading");
    }

    private void Chase()
    {
        unitEventHandler.RaiseEvent("OnMoveOrderIssued", _playerPosition);
    }

    private void Stop()
    {
        unitEventHandler.RaiseEvent("OnMoveOrderIssued", transform.position);
    }

    private float AngleToDestination(Vector3 destination)
    {
        Vector2 angle = destination - transform.position;

        return Mathf.Atan2(angle.y, angle.x) * 180 / Mathf.PI;
    }
}