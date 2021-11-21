using UnityEngine;
using UnityEngine.InputSystem;

public class PressAnyKeyToRestartTheGame : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            LevelManager.Instance.LoadStartGameScene();
        }
    }
}