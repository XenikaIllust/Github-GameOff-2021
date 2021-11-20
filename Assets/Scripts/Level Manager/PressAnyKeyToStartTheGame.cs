using UnityEngine;
using UnityEngine.InputSystem;

public class PressAnyKeyToStartTheGame : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            LevelManager.LoadNextScene();
        }
    }
}