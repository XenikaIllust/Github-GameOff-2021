using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class AbilityButton : MonoBehaviour
{
    // States
    private AbilityButtonState _currentState;

    public AbilityAvailable availableState;
    public AbilityTarget abilityTarget;
    public AbilityCooldown cooldownState;

    // The UIButton component from Canvas.
    private Button uiButton;
    public bool isPressed;

    //public KeyCode abilityKey;
    public char abilityKey;
    public Texture2D targetCursor;

    public Ability ability;
    public float cooldownTimeLive;
    public Image cooldownBG;
    public TMP_Text cooldownTimer;

    private void OnEnable()
    {
        uiButton = GetComponent<Button>();

        availableState = new AbilityAvailable(this);
        abilityTarget = new AbilityTarget(this);
        cooldownState = new AbilityCooldown(this);

        cooldownBG.enabled = false;
        cooldownTimer.enabled = false;

        _currentState = availableState;

        EventManager.StartListening("OnGamePaused", OnGamePaused);
    }

    private void OnDisable()
    {
        EventManager.StopListening("OnGamePaused", OnGamePaused);
    }

    private void OnGamePaused(object @null)
    {
        if (_currentState == abilityTarget)
        {
            SwitchState(_currentState, availableState);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        AutoSwitchState();

        // Run the current state's update loop.
        _currentState.UpdateLoop();

        // Check if user presses the ability key..
        switch (abilityKey)
        {
            case 'q':
                isPressed = Keyboard.current.qKey.isPressed;
                break;
            case 'w':
                isPressed = Keyboard.current.wKey.isPressed;
                break;
            case 'e':
                isPressed = Keyboard.current.eKey.isPressed;
                break;
            case 'r':
                isPressed = Keyboard.current.rKey.isPressed;
                break;
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
        if (cooldownTimeLive > float.Epsilon)
        {
            SwitchState(_currentState, cooldownState);
        }
    }
}
