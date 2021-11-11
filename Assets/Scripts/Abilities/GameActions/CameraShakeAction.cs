using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/Game Action/Camera Shake")]
public class CameraShakeAction : GameAction
{
	public enum ShakeType
	{
		Offset,
		Rotation,
	}

	public ShakeType Type = ShakeType.Offset;
	public float Frequency = 24;
	public AnimationCurve DampingCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
	public Vector3 Direction = Vector3.down;
	public float Duration = 3f;
}
