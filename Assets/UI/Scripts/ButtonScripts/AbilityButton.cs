
using UnityEngine;
using UnityEngine.UI;
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

    public KeyCode abilityKey;

    public Texture2D targetCursor;

    public float cooldownTime;
    public Image cooldownBG;
    public TMP_Text cooldownTimer;

    // Start is called before the first frame update
    void Start()
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
        isPressed = Input.GetKeyDown(abilityKey);
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
