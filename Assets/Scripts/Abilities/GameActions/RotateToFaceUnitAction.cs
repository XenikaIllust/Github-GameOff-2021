using UnityEngine;

public struct RotateToFaceUnitData
{
    public readonly Unit UnitToRotate;
    public Vector3 PointToFace;

    public RotateToFaceUnitData(Unit u, Vector3 pt2Face)
    {
        UnitToRotate = u;
        PointToFace = pt2Face;
    }
}

[CreateAssetMenu(menuName = "Definitions/Game Action/RotateToFaceUnit")]
public class RotateToFaceUnitAction : GameAction
{
    public override void Invoke(object param)
    {
        RotateToFaceUnitData data = (RotateToFaceUnitData)param;
        data.UnitToRotate.TurnAndMove(data.PointToFace);
        Debug.Log("RotateToFacePoint is being executed!");
    }
}