using UnityEngine;
using UnityEngine.InputSystem;

public class YouDiedOption : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            LevelManager.Instance.LoadLastLevel();
        }

        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            LevelManager.Instance.LoadMainMenu();
        }
    }
}
