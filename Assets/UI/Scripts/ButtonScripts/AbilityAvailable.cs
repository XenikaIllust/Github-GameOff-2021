using UnityEngine;

public class AbilityAvailable : AbilityButtonState
{
    public AbilityAvailable(AbilityButton button) : base(button){}

    public override void Enter()
    {
        // You can enter entry actions here.
    }

    public override void UpdateLoop()
    {
        if (AbilityButtonContext.isPressed)
        {
            OnActivated();
            AbilityButtonContext.isPressed = false;
        }
    }

    void OnActivated()
    {
        char keyChar = (char)AbilityButtonContext.abilityKey;

        EventManager.RaiseEvent(keyChar+"Pressed", null);

        Debug.Log(keyChar+"Pressed");

        AbilityButtonContext.SwitchState(AbilityButtonContext.abilityTarget);
    }
}
