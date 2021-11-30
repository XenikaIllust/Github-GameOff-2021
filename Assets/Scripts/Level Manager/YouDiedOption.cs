using UnityEngine;
using UnityEngine.InputSystem;

public class YouDiedOption : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            LevelManager.Instance.LoadLastLevel();
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            LevelManager.Instance.LoadMainMenu();
        }
    }
}
