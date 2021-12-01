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

    [FormerlySerializedAs("healthbarDisplacement")]
    public float healthBarDisplacement = 1f;

    private Unit _playerUnit;
    [SerializeField] private List<AbilityButton> abilityButtons;

    private void Update()
    {
        for (var i = 0; i < abilityButtons.Count; i++)
        {
            abilityButtons[i].cooldownTimeLive = _playerUnit.abilityCooldownList[i];
        }
    }

    private void Awake()
    {
        _playerUnit = FindObjectOfType<PlayerAgent>().GetComponent<Unit>();
        _playerUnit.GetComponent<UnitEventManager>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        Time.timeScale = 1f; // Don't change to 0
        UpdateAbilityInformation();
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
            UpdateAbilityInformation();
            EventManager.RaiseEvent("OnGameResumed", null);
        }

        // Freeze time if game is paused.
        Time.timeScale = 1f * (pauseScreen.activeSelf ? 0 : 1);
    }

    private void UpdateAbilityInformation()
    {
        for (var i = 0; i < abilityButtons.Count; i++)
        {
            abilityButtons[i].ability = _playerUnit.abilities[i];
            abilityButtons[i].abilityIcon.sprite = _playerUnit.abilities[i].abilitySprite;
        }
    }
}
