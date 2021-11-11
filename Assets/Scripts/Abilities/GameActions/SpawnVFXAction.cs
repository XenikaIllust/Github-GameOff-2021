using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/Game Action/Spawn VFX")]
public class SpawnVFXAction : GameAction
{
	public GameObject Prefab;
	public Vector3 Rotation;
	public Vector3 OffsetFromTarget;
	public float Scale = 1f;
	public bool ParentToTarget;
	public string NamedTransformParent;
}
