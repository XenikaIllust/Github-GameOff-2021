using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Definitions/Game Action/VFX/LineIndicatorVFXActionBlock")]
public class LineIndicatorVFXActionBlock : VFXActionBlock
{
    public override void Invoke(float timeToLive, AbilityStatsDict abilityStats, string[] idParams,
        List<object> currentFilteredTargets, Dictionary<string, object> otherTargets)
    {
        /*---------------------------------------------------------------------------------
            The line indicator is cast from the executing unit's front to the target direction
            specified by the length.
            
            idParams[0] is guaranteed to be a string with the name of the VFX in the
            VFX folder if the user has entered the key correctly in the ability editor. 
        ---------------------------------------------------------------------------------*/

        Unit self = (Unit)otherTargets["Executing Unit"];
        Vector2 selfPosition = self.transform.position;
        float selfRotation = self.PseudoObject.transform.rotation.eulerAngles.z;

        LineRenderer lineIndicator = Instantiate(
            Resources.Load<LineRenderer>("VFX/AbilityVFX/LineVFX/LineIndicator/" + idParams[0]), selfPosition,
            Quaternion.Euler(0, 0, selfRotation - 45), self.PseudoObject.transform);

        Destroy(lineIndicator, timeToLive); // hardcoded, change later
    }
}