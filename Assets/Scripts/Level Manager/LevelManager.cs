using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; set; }

    [SerializeField] private SceneReference startGameScene;
    [SerializeField] private SceneReference gameOverScene;

    [SerializeField] private List<SceneReference> levelScenes;

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
    }

    public void LoadStartGameScene()
    {
        SceneManager.LoadScene(startGameScene);
    }

    public void LoadEndGameScene()
    {
        SceneManager.LoadScene(gameOverScene);
    }

    public void LoadLevelScene(int index)
    {
        SceneManager.LoadScene(levelScenes[index - 1]);
    }
}