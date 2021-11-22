using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Definitions/Game Action/Aim")]
public class AimAction : GameActionBlock
{
    public override void Invoke(float abilityStat, List<object> currentFilteredTargets, Dictionary<string, object> otherTargets)
    {
        Unit aimingUnit = (Unit) currentFilteredTargets[0];
        Vector3 aimingTarget = (Vector3) otherTargets["Target Point"];

        // Update the Player's rotation
        // aimingUnit.PseudoObject.transform
        //     .DORotate(new Vector3(float.Epsilon, float.Epsilon, aimingUnit.AngleToTarget(aimingTarget)),
        //         abilityStat * 360)
        //     .SetSpeedBased().SetEase(Ease.Linear);
        
        float eulerAnglesZ = aimingUnit.PseudoObject.transform.rotation.eulerAngles.z;
        aimingUnit.unitEventHandler.RaiseEvent("OnPseudoObjectRotationChanged", eulerAnglesZ);
    }
}
