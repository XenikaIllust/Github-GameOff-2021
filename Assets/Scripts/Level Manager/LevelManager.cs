using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; set; }

    public SceneReference mainMenuScene;
    [SerializeField] private SceneReference youDiedScene;
    [SerializeField] private SceneReference endingScene;

    [SerializeField] private List<SceneReference> destinationLevels;
    [SerializeField] private List<SceneReference> survivalLevels;
    [SerializeField] private List<SceneReference> genocideLevels;
    [SerializeField] private List<SceneReference> eliteLevels;
    [SerializeField] private List<SceneReference> bossLevels;

    [Header("Read Only")] public SceneReference lastLevel;
    private List<List<SceneReference>> _remainingLevels;
    private IEnumerator _pendingLoad;

    private void Awake()
    {
        if (Instance == null || Instance == this)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        RemoveInvalidSceneReferences(destinationLevels);
        RemoveInvalidSceneReferences(survivalLevels);
        RemoveInvalidSceneReferences(genocideLevels);
        RemoveInvalidSceneReferences(eliteLevels);
        RemoveInvalidSceneReferences(bossLevels);
        InitializeRemainingLevels();
    }

    public void InitializeRemainingLevels()
    {
        // Not including boss level
        _remainingLevels = new List<List<SceneReference>>();
        if (destinationLevels.Count > 0) _remainingLevels.Add(destinationLevels);
        if (survivalLevels.Count > 0) _remainingLevels.Add(survivalLevels);
        if (genocideLevels.Count > 0) _remainingLevels.Add(genocideLevels);

        lastLevel = null;
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        EventManager.StartListening("OnPlayerDied", OnPlayerDied);
    }

    private void OnSceneUnloaded(Scene arg0)
    {
        EventManager.StopListening("OnPlayerDied", OnPlayerDied);
        if (_pendingLoad != null) StopCoroutine(_pendingLoad);
    }

    private void OnPlayerDied(object playerGameObject)
    {
        _pendingLoad = DelayedLoad(LoadYouDied, 2);
        StartCoroutine(_pendingLoad);
    }

    public void LoadMainMenu()
    {
        EventManager.RaiseEvent("OnSceneLoading", mainMenuScene);
    }

    public void LoadYouDied()
    {
        EventManager.RaiseEvent("OnSceneLoading", youDiedScene);
    }

    public void LoadLastLevel()
    {
        EventManager.RaiseEvent("OnSceneLoading", lastLevel);
    }

    public void LoadNewGame()
    {
        InitializeRemainingLevels();
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        if (_remainingLevels.Count != 0)
        {
            var nextGameMode = _remainingLevels[Random.Range(0, _remainingLevels.Count)];
            LoadRandomFromCollection(nextGameMode);
            _remainingLevels.Remove(nextGameMode);
        }
        else
        {
            if (bossLevels.Contains(lastLevel))
            {
                EventManager.RaiseEvent("OnSceneLoading", endingScene);
                return;
            }

            if (eliteLevels.Contains(lastLevel))
            {
                LoadRandomFromCollection(bossLevels);
                return;
            }

            LoadRandomFromCollection(eliteLevels);
        }
    }

    private void LoadRandomFromCollection(IReadOnlyList<SceneReference> collection)
    {
        var newLevel = collection[Random.Range(0, collection.Count)];
        EventManager.RaiseEvent("OnSceneLoading", newLevel);
        lastLevel = newLevel;
    }

    private IEnumerator DelayedLoad(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        _pendingLoad = null;
        action.Invoke();
    }

    private void RemoveInvalidSceneReferences(List<SceneReference> list)
    {
        list.RemoveAll(item => item.ScenePath == "");
    }
}
