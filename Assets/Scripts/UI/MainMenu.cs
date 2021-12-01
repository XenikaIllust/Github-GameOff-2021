using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f;
    }

    // Main Menu
    public void Continue()
    {
        if (LevelManager.Instance.lastLevel != null) LevelManager.Instance.LoadLastLevel();
        else StartGame();
    }

    public void StartGame()
    {
        LevelManager.Instance.LoadNewGame();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
