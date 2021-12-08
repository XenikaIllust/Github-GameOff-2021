using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [Header("Settings")] [SerializeField] private float triggerRadius = 10;
    private float _distanceFromPlayer;
    [SerializeField] private float spawnRadius = 2;
    [Space] [SerializeField] private float minSpawnInterval = 1;
    [SerializeField] private float maxSpawnInterval = 5;
    private float _timer = float.Epsilon;
    [Space] public int startingUnit = 5;
    [SerializeField] private int minAliveUnit = 2;
    [SerializeField] private int maxAliveUnit = 10;
    public int lifetimeQuota = 50;
    [Header("Spawnable")] [SerializeField] private GameObject gameObjectPrefab;

    [Header("Read Only")] [SerializeField]
    private List<GameObject> aliveUnits = new List<GameObject>(Array.Empty<GameObject>());

    private void Start()
    {
        var startingNumber = startingUnit;
        while (aliveUnits.Count < startingNumber) Spawn();
        _timer = minSpawnInterval + (maxSpawnInterval - minSpawnInterval) * aliveUnits.Count / maxAliveUnit;
    }

    private void Update()
    {
        if (lifetimeQuota <= 0) return;

        _timer -= Time.deltaTime;

        if (_distanceFromPlayer <= triggerRadius && _timer <= float.Epsilon && aliveUnits.Count < maxAliveUnit)
        {
            if (aliveUnits.Count < minAliveUnit)
            {
                _timer = minSpawnInterval;
                Spawn();
            }
            else
            {
                _timer = minSpawnInterval + (maxSpawnInterval - minSpawnInterval) * aliveUnits.Count / maxAliveUnit;
                Spawn();
            }
        }
    }

    private void OnEnable()
    {
        EventManager.StartListening("OnPlayerPositionChanged", OnPlayerPositionChanged);
        EventManager.StartListening("OnUnitDied", OnUnitDied);
    }

    private void OnDisable()
    {
        EventManager.StopListening("OnPlayerPositionChanged", OnPlayerPositionChanged);
        EventManager.StopListening("OnUnitDied", OnUnitDied);
    }

    private void OnPlayerPositionChanged(object playerPosition)
    {
        _distanceFromPlayer = Vector2.Distance(transform.position, (Vector3)playerPosition);
    }

    private void OnUnitDied(object unit)
    {
        if (aliveUnits.Contains((GameObject)unit)) aliveUnits.Remove((GameObject)unit);
    }

    private void Spawn()
    {
        var unitGO = Instantiate(gameObjectPrefab);
        if(LoadingManager.Instance) 
        {
            SceneManager.MoveGameObjectToScene(unitGO,
                SceneManager.GetSceneByPath(LoadingManager.Instance.currentScene.ScenePath));
        }
        aliveUnits.Add(unitGO);
        lifetimeQuota -= 1;
        Vector3 offset = new Vector2(Random.Range(0, spawnRadius), Random.Range(0, spawnRadius));
        Unit unit = unitGO.GetComponent<Unit>();
        unit.agent.Warp(new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, 0));
        unit.transform.position = transform.position + offset;
    }
}
