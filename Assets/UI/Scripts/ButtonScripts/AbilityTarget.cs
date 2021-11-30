using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityTarget : AbilityButtonState
{
    public AbilityTarget(AbilityButton button) : base(button)
    {
    }

    public override void Enter()
    {
        Cursor.SetCursor(AbilityButtonContext.targetCursor, Vector2.zero, CursorMode.Auto);
    }

    public override void UpdateLoop()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            AbilityButtonContext.SwitchState(this, AbilityButtonContext.cooldownState);
        }
    }

    public override void Leave()
    {
        // You can exit entry actions here.
    }
}
