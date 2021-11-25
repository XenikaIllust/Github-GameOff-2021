using UnityEngine;

public abstract class AbilityButtonState
{
    protected AbilityButton _abilityButton;

    public AbilityButtonState(AbilityButton context)
    {
        _abilityButton = context;
    }

    public AbilityButton AbilityButtonContext{ get{return _abilityButton;} set{_abilityButton = value;} }

    public abstract void Enter();

    public abstract void UpdateLoop();
}
