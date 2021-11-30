using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Definitions/Game Action/VFX/AOEVFXActionBlock")]
public class AOEVFXActionBlock : VFXActionBlock
{
    public override void Invoke(float unused, AbilityStatsDict abilityStats, string[] idParams, List<object> currentFilteredTargets, Dictionary<string, object> otherTargets) {
        /*---------------------------------------------------------------------------------
            idParams[0] is guaranteed to be a string with the name of the VFX in the
            VFX folder if the user has entered the key correctly in the ability editor. 

            idParams[1] is guaranteed to be a Vector3 point for AOEVFXActionBlock if the user
            has entered the key correctly in the ability editor.
        ---------------------------------------------------------------------------------*/

        Vector2 spawnPoint = (Vector3) otherTargets[idParams[1]]; 
        ParticleSystem vfx = Instantiate<ParticleSystem>( Resources.Load<ParticleSystem>(libraryPrefix + "AOEVFX/" + idParams[0]), spawnPoint, Quaternion.identity );
        MathUtils.ConvertCircleToIsometricCircle(abilityStats["AOE Radius"], vfx.gameObject);
        vfx.Play();
    }
}
