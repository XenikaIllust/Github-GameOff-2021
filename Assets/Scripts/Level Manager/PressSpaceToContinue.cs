using UnityEngine;

public class PressSpaceToContinue : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LevelManager.LoadNextScene();
        }
    }
}