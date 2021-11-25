using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameAction
{
    public GameActionBlock GameActionBlock;

    // for GameActionBlocks that require a stat
    public string StatId;

    // for VFX GameActionBlocks that require inputs
    public string VFXName;

    // Point VFX
    public string VFXPointId;

    // Line VFX
    public string VFXStartPointId;
    public string VFXEndPointId;

    // Unit VFX
    public string VFXUnitId;

    // for SFX GameActionBlocks that require inputs
    public string SFXName;

    // Name of Projectile Prefab
    public string ProjectileName;

    // for AnimationAction
    public string AnimationName;

    public void Invoke(AbilityStatsDict AbilityStats, List<object> CurrentFilteredTargets, Dictionary<string, object> AllTargets) {
        if(AbilityStats.ContainsKey(StatId)) { // this GameActionBlock type requires a stat
            GameActionBlock.Invoke(AbilityStats[StatId], CurrentFilteredTargets, AllTargets);
        }
        else if(GameActionBlock.GetType() == typeof(PointVFXActionBlock)) {
            string[] idParams = new string[] { VFXName, VFXPointId };
            GameActionBlock.Invoke(idParams, CurrentFilteredTargets, AllTargets);
        }
        else if(GameActionBlock.GetType() == typeof(SFXActionBlock)) {
            string[] idParams = new string[] { SFXName };
            GameActionBlock.Invoke(idParams, CurrentFilteredTargets, AllTargets);
        }
        else if(GameActionBlock.GetType() == typeof(FireProjectileAction)) {
            string[] idParams = new string[] { ProjectileName };
            GameActionBlock.Invoke(idParams, CurrentFilteredTargets, AllTargets);
        }
        else if(GameActionBlock.GetType() == typeof(AnimationAction)) {
            string[] idParams = new string[] { AnimationName };
            GameActionBlock.Invoke(idParams, CurrentFilteredTargets, AllTargets);
        }
        else {  // this GameActionBlock type requires no stat
            GameActionBlock.Invoke(CurrentFilteredTargets, AllTargets);
        }
    }
}
