using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameActionBlock : ScriptableObject
{
	public virtual void Invoke(Dictionary<string, float> abilityStats, List<object> currentFilteredTargets, Dictionary<string, object> otherTargets) { }
}
