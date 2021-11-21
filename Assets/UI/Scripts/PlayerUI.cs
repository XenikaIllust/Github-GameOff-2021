
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    public GameObject PauseScreen;

    public KeyCode pauseKey = KeyCode.Escape;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PauseGame();
    }

    
    void PauseGame()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            // Toggle the pause screen
            PauseScreen.SetActive(!PauseScreen.activeSelf);
            
            // If and only if the pause screen is active,
            // then raise the OnGamePaused event.
            if (PauseScreen.activeSelf)
            {
                EventManager.RaiseEvent("OnGamePaused", null);
            }

            // Freeze time if game is paused.
            Time.timeScale = 1f * (PauseScreen.activeSelf == true ? 0 : 1);

        }
    }
}
