using UnityEngine;

public class ActionBar : MonoBehaviour
{
    // For testing only...
    void Update()
    {
        bool isQPressed = Input.GetKeyDown(KeyCode.Q);
        bool isWPressed = Input.GetKeyDown(KeyCode.W);
        bool isEPressed = Input.GetKeyDown(KeyCode.E);
        bool isRPressed = Input.GetKeyDown(KeyCode.R);

        if (isQPressed) OnQPress();
        if (isWPressed) OnWPress();
        if (isEPressed) OnEPress();
        if (isRPressed) OnRPress();
    }

    // Button event handling...
    public void OnQPress()
    {
        EventManager.RaiseEvent("QPressed", null);
    }

    public void OnWPress()
    {
        EventManager.RaiseEvent("WPressed", null);
    }

    public void OnEPress()
    {
        EventManager.RaiseEvent("EPressed", null);
    }

    public void OnRPress()
    {
        EventManager.RaiseEvent("RPressed", null);
    }
}