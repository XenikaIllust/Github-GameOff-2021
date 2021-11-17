
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityCooldown : AbilityButtonState
{
    public AbilityCooldown(AbilityButton button) : base(button) {}

    public override void Enter()
    {
        AbilityButtonContext.cooldownBG.enabled = true;
        AbilityButtonContext.cooldownTimer.enabled = true;
    }

    public override void UpdateLoop()
    {
        TimeIt();
        StopTime();
    }

    // Timing section

    float timeAccum = 0f;

    void TimeIt()
    {
        Image cooldownBG = AbilityButtonContext.cooldownBG;
        TMP_Text cooldownTimer = AbilityButtonContext.cooldownTimer;
        float cooldownTime = AbilityButtonContext.cooldownTime;

        cooldownBG.fillAmount = (cooldownTime - timeAccum)/cooldownTime;

        cooldownTimer.text = string.Format("{0}", Mathf.RoundToInt(cooldownTime - timeAccum));

        timeAccum += Time.deltaTime;
    }

    void StopTime()
    {
        if (timeAccum >= AbilityButtonContext.cooldownTime)
        {
            AbilityButtonContext.cooldownBG.fillAmount = 0;
            AbilityButtonContext.cooldownTimer.text = "";

            AbilityButtonContext.cooldownBG.enabled = false;
            AbilityButtonContext.cooldownTimer.enabled = false;

            timeAccum = 0;
            AbilityButtonContext.SwitchState(AbilityButtonContext.availableState);
        }
    }
}
