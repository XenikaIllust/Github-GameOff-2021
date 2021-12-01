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
        var context = AbilityButtonContext;
        context.cooldownBG.fillAmount = context.cooldownTimeLive / context.ability.cooldown;
        if (context.cooldownTimeLive <= float.Epsilon) context.SwitchState(this, context.availableState);
    }

    private void StopTime()
    {
        AbilityButtonContext.cooldownBG.fillAmount = 0;
        AbilityButtonContext.cooldownBG.enabled = false;
    }
}
