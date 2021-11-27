using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class AbilityButton : MonoBehaviour
{
    // States
    AbilityButtonState _currentState;

    public AbilityAvailable availableState;
    public AbilityTarget abilityTarget;
    public AbilityCooldown cooldownState;

    // The UIButton component from Canvas.
    Button uiButton;

    public bool isPressed = false;

    //public KeyCode abilityKey;
    public char abilityKey;

    public Texture2D targetCursor;

    public float cooldownTime;
    public Image cooldownBG;
    public TMP_Text cooldownTimer;

    // OnEnable is called when this game object is activated
    void OnEnable()
    {
        uiButton = GetComponent<Button>();

        availableState = new AbilityAvailable(this);
        abilityTarget = new AbilityTarget(this);
        cooldownState = new AbilityCooldown(this);

        cooldownBG.enabled = false;
        cooldownTimer.enabled = false;

        _currentState = availableState;
    }

    // Update is called once per frame
    void Update()
    {
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
            default:
                break;
        }
    }

    // UI Button callback function
    public void OnButtonPressed()
    {
        isPressed = true;
    }

    public void SwitchState(AbilityButtonState newState)
    {
        newState.Enter();
        _currentState = newState;
    }
}
