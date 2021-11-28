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
            SpriteRenderer targetUnitSpriteRenderer = targetUnit.transform.GetComponentInChildren<SpriteRenderer>(); 
            Vector2 spawnPoint = targetUnitSpriteRenderer.bounds.center; // for sprite center
            Debug.Log(libraryPrefix + "UnitVFX/" + idParams[0]);
            ParticleSystem vfx = Instantiate<ParticleSystem>( Resources.Load<ParticleSystem>(libraryPrefix + "UnitVFX/" + idParams[0]), spawnPoint, Quaternion.identity, targetUnit.transform );
            vfx.Play();
        }
    }
}
