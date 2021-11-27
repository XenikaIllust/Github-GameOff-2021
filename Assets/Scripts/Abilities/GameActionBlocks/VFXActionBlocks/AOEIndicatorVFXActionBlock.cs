using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Definitions/Game Action/VFX/AOEIndicatorVFXActionBlock")]
public class AOEIndicatorVFXActionBlock : VFXActionBlock
{
    public override void Invoke(float timeToLive, AbilityStatsDict abilityStats, string[] idParams, List<object> currentFilteredTargets, Dictionary<string, object> otherTargets) {
        /*---------------------------------------------------------------------------------
            idParams[0] is guaranteed to be a string with the name of the VFX in the
            VFX folder if the user has entered the key correctly in the ability editor. 
            
            idParams[1] is guaranteed to be the executing unit for AOEIndicatorAction if the 
            user has entered the key correctly in the ability editor.
        ---------------------------------------------------------------------------------*/

        // temporary, may want to change this to a particle system later
        Unit self = (Unit) otherTargets[idParams[1]]; 
        Vector2 selfPosition = self.transform.position;
        float selfRotation = self.PseudoObject.transform.rotation.eulerAngles.z;
        GameObject AOEIndicatorParent = new GameObject("AOEIndicatorParent");
        AOEIndicatorParent.transform.position = selfPosition;
        GameObject AOEIndicator = Instantiate<GameObject>(Resources.Load<GameObject>("VFX/AbilityVFX/AOEVFX/AOEIndicator/AOEIndicator"), selfPosition, Quaternion.identity, AOEIndicatorParent.transform);
        AOEIndicatorParent.transform.position = self.transform.position;
        ConvertCircleToIsometricCircle( abilityStats["AOE Radius"] , AOEIndicator);
        var rot = AOEIndicatorParent.transform.rotation.eulerAngles;
        rot.z = selfRotation;
        AOEIndicatorParent.transform.rotation = Quaternion.Euler(rot);

        Destroy(AOEIndicatorParent, timeToLive); // hardcoded, change later
    }

    void ConvertCircleToIsometricCircle(float radius, GameObject circularIndicatorObject) {
        circularIndicatorObject.transform.localScale = new Vector3(radius * 0.825f, radius * 0.5f * 0.825f, 1.0f); // this assumes the graphic is 512x512, and pixels per unit is 256!
    }
}
