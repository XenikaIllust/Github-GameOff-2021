using UnityEngine;

public class PressAnyKeyToRestartTheGame : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            LevelManager.LoadNextScene();
        }
    }
}