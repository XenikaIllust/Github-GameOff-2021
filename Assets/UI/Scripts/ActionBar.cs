
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
        Debug.Log("Q has been pressed.");
        EventManager.RaiseEvent("QPressed", null);
    }

    public void OnWPress()
    {
        Debug.Log("W has been pressed.");
        EventManager.RaiseEvent("WPressed", null);
    }

    public void OnEPress()
    {
        Debug.Log("E has been pressed.");
        EventManager.RaiseEvent("EPressed", null);
    }

    public void OnRPress()
    {
        Debug.Log("R has been pressed.");
        EventManager.RaiseEvent("RPressed", null);
    }

}
