using UnityEngine.UI;

public class AbilityCooldown : AbilityButtonState
{
    public AbilityCooldown(AbilityButton button) : base(button)
    {
    }

    public override void Enter()
    {
        AbilityButtonContext.cooldownBG.enabled = true;
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

        cooldownBG.fillAmount = AbilityButtonContext.cooldownTimeLive / AbilityButtonContext.ability.cooldown;

        if (AbilityButtonContext.cooldownTimeLive <= float.Epsilon)
        {
            AbilityButtonContext.SwitchState(this, AbilityButtonContext.availableState);
        }
    }

    private void StopTime()
    {
        AbilityButtonContext.cooldownBG.fillAmount = 0;
        AbilityButtonContext.cooldownBG.enabled = false;
    }
}
