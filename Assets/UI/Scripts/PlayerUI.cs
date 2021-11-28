using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerUI : MonoBehaviour
{
    public GameObject PauseScreen;

    public Slider EnemyHealthBarPrefab;
    //private int numEnemies = 0;
    public float healthbarDisplacement = 1f;

    public UnitEventManager unitEventManager;

    // Start is called before the first frame update
    void Start()
    {
        //unitEventManager.UnitEventHandler.StartListening("OnSpawned", ShowEnemyHealth);

        // Set time scale to 1 so that the game starts unpaused.
        // There is currently a weird bug where if you don't set
        // this, the game starts frozen as if it is paused.
        Time.timeScale = 1f;
    }

    // 'Escape' Key, 'P' Key
    public void OnPauseGamePressed(InputAction.CallbackContext context)
    {
        if (context.started) // Button Pressed
        {
            // Toggle the pause screen
            PauseScreen.SetActive(!PauseScreen.activeSelf);
            
            // If and only if the pause screen is active,
            // then raise the OnGamePaused event.
            if (PauseScreen.activeSelf)
            {
                EventManager.RaiseEvent("OnGamePaused", null);
            }
            else
            {
                EventManager.RaiseEvent("OnGameResumed", null);
            }

            // Freeze time if game is paused.
            Time.timeScale = 1f * (PauseScreen.activeSelf == true ? 0 : 1);
        }
    }

    // Display enemy health if there are enemies in the scene...
    void ShowEnemyHealth(object param)
    {
        GameObject enemy = (GameObject)param;

        // TODO: if (enemy has no healthbar then do this)
        Vector2 enemyScreenPosition = Camera.main.WorldToScreenPoint(enemy.GetComponent<Transform>().position);

        Vector2 healthBarPosition = Camera.main.WorldToScreenPoint((Vector2)enemy.GetComponent<Transform>().position + (Vector2.up * healthbarDisplacement));

        // Instantiate a new health bar just above their head.
        Slider newHealthBar = (Slider)Instantiate(
            EnemyHealthBarPrefab,
            healthBarPosition,
            Quaternion.identity
        );

        // Parent the health bar to this Canvas.
        newHealthBar.transform.SetParent(transform);

        // Set the enemy instance to the health bar's reference variable.
        newHealthBar.GetComponent<EnemyHealthBar>().EnemyInstance = enemy;

        newHealthBar.GetComponent<EnemyHealthBar>().DisplacementPosition = Vector2.up * healthbarDisplacement;
    }
}
