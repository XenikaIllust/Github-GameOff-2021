public class AbilityAvailable : AbilityButtonState
{
    public AbilityAvailable(AbilityButton button) : base(button)
    {
    }

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

    public override void Leave()
    {
        // You can exit entry actions here.
    }

    private void OnActivated()
    {
        char keyChar = AbilityButtonContext.abilityKey;

        EventManager.RaiseEvent(char.ToUpper(keyChar) + "Pressed", null);

        if (AbilityButtonContext.ability.inputType != AbilityType.NoTarget)
        {
            AbilityButtonContext.SwitchState(this, AbilityButtonContext.AbilityTarget);
        }
    }
}
