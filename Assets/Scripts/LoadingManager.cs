using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance { get; set; }
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingSlider;
    private SceneReference _currentScene;
    private readonly List<AsyncOperation> _scenesLoading = new List<AsyncOperation>();

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        // Load the Main Menu
        SceneManager.LoadSceneAsync(LevelManager.Instance.mainMenuScene, LoadSceneMode.Additive);
        _currentScene = LevelManager.Instance.mainMenuScene;
    }

    private void OnEnable()
    {
        EventManager.StartListening("OnSceneLoading", OnSceneLoading);
    }

    private void OnDisable()
    {
        EventManager.StopListening("OnSceneLoading", OnSceneLoading);
    }

    private void OnSceneLoading(object sceneReference)
    {
        loadingScreen.SetActive(true); // Show the Loading Screen
        _scenesLoading.Add(SceneManager.UnloadSceneAsync(_currentScene)); // Unload the previous scene

        // Load the new scene
        _scenesLoading.Add(SceneManager.LoadSceneAsync((SceneReference)sceneReference, LoadSceneMode.Additive));
        _currentScene = (SceneReference)sceneReference;

        StartCoroutine(GetSceneLoadProgress());
    }

    private IEnumerator GetSceneLoadProgress()
    {
        foreach (var asyncOperation in _scenesLoading)
        {
            while (!asyncOperation.isDone)
            {
                var totalSceneProgress = _scenesLoading.Sum(operation => operation.progress);

                totalSceneProgress = totalSceneProgress / _scenesLoading.Count * 100f;

                loadingSlider.value = Mathf.FloorToInt(totalSceneProgress);

                yield return null;
            }
        }

        loadingScreen.SetActive(false);
    }
}
