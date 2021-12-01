using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button continueButton;

    private void Start()
    {
        Time.timeScale = 1f;
        if (LevelManager.Instance.lastLevel == null)
        {
            continueButton.interactable = false;
        }
    }

    // Main Menu
    public void Continue()
    {
        LevelManager.Instance.LoadLastLevel();
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
