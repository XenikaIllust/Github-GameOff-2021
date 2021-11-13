using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public struct RotateToFaceUnitData {
    public Unit unitToRotate;
    public Vector3 pointToFace;

    public RotateToFaceUnitData(Unit u, Vector3 pt2Face) {
        unitToRotate = u;
        pointToFace = pt2Face;
    }
}

[CreateAssetMenu(menuName = "Definitions/Game Action/RotateToFaceUnit")]
public class RotateToFaceUnitAction : GameAction
{
    // public override void Invoke(List<object> requiredInput, List<object> targets)
    // {
    //     // RotateToFaceUnitData data = (RotateToFaceUnitData) param;
    //     // data.unitToRotate.RotateToFacePoint(data.pointToFace);
    //     // Debug.Log("RotateToFacePoint is being executed!");
    // }
} 
