using UnityEngine;
using UnityEngine.InputSystem;

public class ActionBar : MonoBehaviour
{
    /*  
    These functions have to be called from the Player Input Component
    If this script is only meant to raise events based on input, then it shouldn't exist
    and just handle this logic inside Unit.cs
    */

    // 'Q' Key
    public void OnQPressed(InputAction.CallbackContext context)
    {
        if (context.started) // Button Pressed
        {
            EventManager.RaiseEvent("QPressed", null);
        }
    }

    // 'W' Key
    public void OnWPressed(InputAction.CallbackContext context)
    {
        if (context.started) // Button Pressed
        {
            EventManager.RaiseEvent("WPressed", null);
        }
    }

    // 'E' Key
    public void OnEPressed(InputAction.CallbackContext context)
    {
        if (context.started) // Button Pressed
        {
            EventManager.RaiseEvent("EPressed", null);
        }
    }

    // 'R' Key
    public void OnRPressed(InputAction.CallbackContext context)
    {
        if (context.started) // Button Pressed
        {
            EventManager.RaiseEvent("RPressed", null);
        }
    }
}
