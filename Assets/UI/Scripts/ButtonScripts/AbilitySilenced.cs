using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilitySilenced : AbilityButtonState
{
    public AbilitySilenced(AbilityButton button) : base(button)
    {
    }

    public override void Enter()
    {
        StartSilence();
    }

    public override void UpdateLoop()
    {
        TimeSilence();
    }

    public override void Leave()
    {
        StopSilence();
    }

    private void StartSilence() {
        AbilityButtonContext.silenceBG.enabled = true;
    }

    private void TimeSilence()
    {
        if (AbilityButtonContext.silenceTimeLive <= float.Epsilon)
        {
            AbilityButtonContext.SwitchState(this, AbilityButtonContext.AvailableState);
        }
    }

    private void StopSilence()
    {
        AbilityButtonContext.silenceBG.enabled = false;
    }
}
