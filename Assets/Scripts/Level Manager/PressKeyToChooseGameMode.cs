using UnityEngine;
using UnityEngine.InputSystem;

public class PressKeyToChooseGameMode : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            LevelManager.Instance.LoadLevelScene(1);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            LevelManager.Instance.LoadLevelScene(2);
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            LevelManager.Instance.LoadLevelScene(3);
        }

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            LevelManager.Instance.LoadLevelScene(4);
        }
    }
}