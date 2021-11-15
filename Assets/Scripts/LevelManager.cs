using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void LoadRandomScene()
    {
        SceneManager.LoadScene(Random.Range(0, SceneManager.sceneCountInBuildSettings));
    }
}