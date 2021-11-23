using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Definitions/Game Action/VFX/ArcIndicatorActionBlock")]
public class ArcIndicatorVFXAction : VFXActionBlock
{
    public override void Invoke(string[] idParams, List<object> currentFilteredTargets, Dictionary<string, object> otherTargets) {
        /*---------------------------------------------------------------------------------
            idParams[0] is guaranteed to be a string with the name of the VFX in the
            VFX folder if the user has entered the key correctly in the ability editor. 
            
            idParams[1] is guaranteed to be the executing unit for ArcIndicatorAction if the 
            user has entered the key correctly in the ability editor.
        ---------------------------------------------------------------------------------*/

        // temporary, may want to change this to a particle system later
        Unit self = (Unit) otherTargets[idParams[1]]; 
        Vector2 selfPosition = self.transform.position;
        float selfRotation = self.PseudoObject.transform.rotation.eulerAngles.z;
        GameObject arcIndicatorParent = new GameObject("ArcIndicatorParent");
        arcIndicatorParent.transform.position = self.transform.position;
        SpriteRenderer arcIndicator = Instantiate<SpriteRenderer>( Resources.Load<SpriteRenderer>(libraryPrefix + "ArcVFX/ArcIndicator/" + idParams[0]), selfPosition, Quaternion.identity, arcIndicatorParent.transform );
        var rot = arcIndicator.transform.rotation.eulerAngles;
        rot.z = selfRotation;
        arcIndicator.transform.rotation = Quaternion.Euler(rot);

        Destroy(arcIndicatorParent, 2);
    }
}
