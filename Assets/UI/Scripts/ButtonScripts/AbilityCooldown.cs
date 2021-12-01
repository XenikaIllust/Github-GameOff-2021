using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityCooldown : AbilityButtonState
{
    public AbilityCooldown(AbilityButton button) : base(button)
    {
    }

    public override void Enter()
    {
        AbilityButtonContext.cooldownBG.enabled = true;
        AbilityButtonContext.cooldownTimer.enabled = true;
    }

    public override void UpdateLoop()
    {
        TimeIt();
    }

    public override void Leave()
    {
        StopTime();
    }

    private void TimeIt()
    {
        Image cooldownBG = AbilityButtonContext.cooldownBG;
        TMP_Text cooldownTimer = AbilityButtonContext.cooldownTimer;

        cooldownBG.fillAmount = AbilityButtonContext.cooldownTimeLive / AbilityButtonContext.ability.cooldown;
        cooldownTimer.text = $"{Mathf.CeilToInt(AbilityButtonContext.cooldownTimeLive)}";

        if (AbilityButtonContext.cooldownTimeLive <= float.Epsilon)
        {
            AbilityButtonContext.SwitchState(this, AbilityButtonContext.AvailableState);
        }
    }

    private void StopTime()
    {
        AbilityButtonContext.cooldownBG.fillAmount = 0;
        AbilityButtonContext.cooldownTimer.text = "";

        AbilityButtonContext.cooldownBG.enabled = false;
        AbilityButtonContext.cooldownTimer.enabled = false;
    }
}
