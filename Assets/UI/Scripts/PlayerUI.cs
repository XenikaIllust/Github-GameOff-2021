using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerUI : MonoBehaviour
{
    [FormerlySerializedAs("PauseScreen")] public GameObject pauseScreen;

    [FormerlySerializedAs("EnemyHealthBarPrefab")]
    public Slider enemyHealthBarPrefab;

    //private int numEnemies = 0;
    [FormerlySerializedAs("healthbarDisplacement")]
    public float healthBarDisplacement = 1f;

    private UnitEventManager _unitEventManager;
    private Unit _playerUnit;
    [SerializeField] private List<AbilityButton> abilityButtons;

    private void Update()
    {
        for (var i = 0; i < abilityButtons.Count; i++)
        {
            abilityButtons[i].ability = _playerUnit.abilities[i];
            abilityButtons[i].cooldownTimeLive = _playerUnit.abilityCooldownList[i];
            abilityButtons[i].abilityIcon.sprite = _playerUnit.abilities[i].abilitySprite;
        }
    }

    private void Awake()
    {
        _playerUnit = FindObjectOfType<PlayerAgent>().GetComponent<Unit>();
        _unitEventManager = _playerUnit.GetComponent<UnitEventManager>();
    }

    // Start is called before the first frame update
    private void Start()
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
            PauseGame();
        }
    }

    public void PauseGame()
    {
        // Toggle the pause screen
        pauseScreen.SetActive(!pauseScreen.activeSelf);

        // If and only if the pause screen is active,
        // then raise the OnGamePaused event.
        if (pauseScreen.activeSelf)
        {
            EventManager.RaiseEvent("OnGamePaused", null);
        }
        else
        {
            EventManager.RaiseEvent("OnGameResumed", null);
        }

        // Freeze time if game is paused.
        Time.timeScale = 1f * (pauseScreen.activeSelf ? 0 : 1);
    }

    // Display enemy health if there are enemies in the scene...
    private void ShowEnemyHealth(object param)
    {
        GameObject enemy = (GameObject)param;

        // TODO: if (enemy has no health bar then do this)
        Vector2 enemyScreenPosition = Camera.main.WorldToScreenPoint(enemy.GetComponent<Transform>().position);

        Vector2 healthBarPosition = Camera.main.WorldToScreenPoint((Vector2)enemy.GetComponent<Transform>().position
                                                                   + (Vector2.up * healthBarDisplacement));

        // Instantiate a new health bar just above their head.
        Slider newHealthBar = Instantiate(enemyHealthBarPrefab, healthBarPosition, Quaternion.identity);

        // Parent the health bar to this Canvas.
        newHealthBar.transform.SetParent(transform);

        // Set the enemy instance to the health bar's reference variable.
        newHealthBar.GetComponent<EnemyHealthBar>().EnemyInstance = enemy;

        newHealthBar.GetComponent<EnemyHealthBar>().DisplacementPosition = Vector2.up * healthBarDisplacement;
    }
}
