using System.Collections.Generic;

[System.Serializable]
public class InflictDamageAction : GameActionBlock
{
    public override void Invoke(float abilityStat, List<object> currentFilteredTargets,
        Dictionary<string, object> otherTargets)
    {
        //hardcoded damage, change it later
        foreach (object target in currentFilteredTargets)
        {
            Unit targetUnit = (Unit)target;
            float damageStat = abilityStat;
            targetUnit.UnitEventHandler.RaiseEvent("OnDamageTaken", damageStat);
        }
    }
}