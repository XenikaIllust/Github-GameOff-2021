using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Definitions/Game Action/VFX/UnitVFXActionBlock")]
public class UnitVFXActionBlock : VFXActionBlock
{
    public override void Invoke(string[] idParams, List<object> currentFilteredTargets, Dictionary<string, object> otherTargets)
    {
        foreach (object target in currentFilteredTargets)
        {
            Unit targetUnit = (Unit)target;
            // Vector2 spawnPoint = (Vector3) otherTargets[idParams[1]]; 
            Vector2 spawnPoint = targetUnit.transform.position;
            ParticleSystem vfx = Instantiate<ParticleSystem>( Resources.Load<ParticleSystem>(libraryPrefix + "UnitVFX/" + idParams[0]), spawnPoint, Quaternion.identity, targetUnit.transform );
            vfx.Play();
        }
    }
}
