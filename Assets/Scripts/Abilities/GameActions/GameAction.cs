using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameActionBehaviour {
    Autofire,
    ConstantUpdate
}

public class GameAction : ScriptableObject
{
	public GameActionBehaviour gameActionBehaviour;

	public virtual void Invoke(object param) { }
}
