using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAction : ScriptableObject
{
	public virtual void Invoke(Dictionary<string, object> inputTargets, List<object> targets) { }
}
