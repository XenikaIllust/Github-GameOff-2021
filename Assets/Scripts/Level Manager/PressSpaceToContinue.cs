using UnityEngine;
using UnityEngine.InputSystem;

public class PressSpaceToContinue : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            LevelManager.Instance.LoadEndGameScene();
        }
    }
}