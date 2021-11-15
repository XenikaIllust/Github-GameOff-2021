using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameAction
{
    public GameActionBlock GameActionBlock;
    public string StatId;

    public void Invoke(AbilityStatsDict AbilityStats, List<object> CurrentFilteredTargets, Dictionary<string, object> AllTargets) {
        if(AbilityStats.ContainsKey(StatId)) {
            GameActionBlock.Invoke(AbilityStats[StatId], CurrentFilteredTargets, AllTargets);
        }
        else {
            GameActionBlock.Invoke(0, CurrentFilteredTargets, AllTargets);
        }
    }
}
