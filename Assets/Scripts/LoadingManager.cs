using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;
    [SerializeField]private GameObject loadingScreen;
    [SerializeField]private Slider loadingSlider;
    private SceneReference _currentScene;
    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    private void Awake()
    {
        instance = this;

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
        scenesLoading.Add(SceneManager.UnloadSceneAsync(_currentScene)); // Unload the previous scene

        // Load the new scene
        scenesLoading.Add(SceneManager.LoadSceneAsync((SceneReference)sceneReference, LoadSceneMode.Additive));
        _currentScene = (SceneReference)sceneReference;

        StartCoroutine(GetSceneLoadProgress());
    }

    private IEnumerator GetSceneLoadProgress()
    {
        for(int i=0; i<scenesLoading.Count; i++)
        {
            while(!scenesLoading[i].isDone)
            {
                float totalSceneProgress = 0;

                foreach(AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }

                totalSceneProgress = (totalSceneProgress/scenesLoading.Count) * 100f;

                loadingSlider.value = Mathf.RoundToInt(totalSceneProgress);

                yield return null;
            }
        }

        loadingScreen.SetActive(false);
    }
}
