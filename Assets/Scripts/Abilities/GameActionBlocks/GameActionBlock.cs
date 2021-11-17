using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class GameActionBlock : ScriptableObject
{
	public virtual void Invoke(float abilityStat, List<object> currentFilteredTargets, Dictionary<string, object> otherTargets) { } // with stat variants
	public virtual void Invoke(List<object> currentFilteredTargets, Dictionary<string, object> otherTargets) { } // no stat variants
	public virtual void Invoke(string[] idParams, List<object> currentFilteredTargets, Dictionary<string, object> otherTargets) { } // vfx and sfx variants
}
