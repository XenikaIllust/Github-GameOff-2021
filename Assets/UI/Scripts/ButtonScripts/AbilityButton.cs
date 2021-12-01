using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class AbilityButton : MonoBehaviour
{
    // States
    private AbilityButtonState _currentState;

    public AbilityAvailable AvailableState;
    public AbilityTarget AbilityTarget;
    public AbilityCooldown CooldownState;

    // The UIButton component from Canvas.
    public bool isPressed;

    //public KeyCode abilityKey;
    public char abilityKey;
    public Texture2D targetCursor;

    public Ability ability;
    public float cooldownTimeLive;
    public Image abilityIcon;
    public Image cooldownBG;
    public TMP_Text cooldownTimer;
    private Unit _playerUnit;

    private void OnEnable()
    {
        _playerUnit = FindObjectOfType<PlayerAgent>().GetComponent<Unit>();
        abilityIcon = GetComponent<Image>();

        AvailableState = new AbilityAvailable(this);
        AbilityTarget = new AbilityTarget(this);
        CooldownState = new AbilityCooldown(this);

        cooldownBG.enabled = false;
        cooldownTimer.enabled = false;

        _currentState = AvailableState;

        EventManager.StartListening("OnGamePaused", OnGamePaused);
    }

    private void OnDisable()
    {
        EventManager.StopListening("OnGamePaused", OnGamePaused);
    }

    private void OnGamePaused(object @null)
    {
        if (_currentState == AbilityTarget) SwitchState(_currentState, AvailableState);
    }

    // Update is called once per frame
    private void Update()
    {
        AutoSwitchState();

        // Run the current state's update loop.
        _currentState.UpdateLoop();

        // Check if user presses the ability key..
        if (_playerUnit.inputLockDuration <= float.Epsilon)
        {
            isPressed = abilityKey switch
            {
                'q' => Keyboard.current.qKey.isPressed,
                'w' => Keyboard.current.wKey.isPressed,
                'e' => Keyboard.current.eKey.isPressed,
                'r' => Keyboard.current.rKey.isPressed,
                _ => isPressed
            };
        }
    }

    // UI Button callback function
    public void OnButtonPressed()
    {
        isPressed = true;
    }

    public void SwitchState(AbilityButtonState oldState, AbilityButtonState newState)
    {
        oldState.Leave();
        newState.Enter();
        _currentState = newState;
    }

    private void AutoSwitchState()
    {
        if (cooldownTimeLive > float.Epsilon) SwitchState(_currentState, CooldownState);
    }
}
