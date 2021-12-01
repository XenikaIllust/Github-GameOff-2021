using UnityEngine;

public class CreditsMenu : MonoBehaviour
{
    public void BackToMainMenu()
    {
        LevelManager.Instance.LoadMainMenu();
    }
}
