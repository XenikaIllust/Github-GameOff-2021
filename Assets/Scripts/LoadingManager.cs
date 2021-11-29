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
    private int currentSceneIndex = 0;
    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    private void Awake()
    {
        instance = this;

        // Load the Main Menu
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        currentSceneIndex = 1;
    }

    private void OnEnable()
    {
        EventManager.StartListening("OnLoadScene", LoadScene);
    }

    private void OnDisable()
    {
        EventManager.StopListening("OnLoadScene", LoadScene);
    }

    private void LoadScene(object sceneIndex)
    {
        loadingScreen.SetActive(true); // Show the Loading Screen
        scenesLoading.Add(SceneManager.UnloadSceneAsync(currentSceneIndex)); // Unload the previous scene

        // Load the new scene
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)sceneIndex, LoadSceneMode.Additive));
        currentSceneIndex = (int)sceneIndex;

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
