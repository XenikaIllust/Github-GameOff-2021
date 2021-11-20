using UnityEngine;

public class PressAnyKeyToStartTheGame : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            LevelManager.LoadPreviousScene();
        }
    }
}