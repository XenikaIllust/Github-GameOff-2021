using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/Game Action/Fire Projectile")]
public class FireProjectileAction : GameActionBlock
{
    public override void Invoke(string[] idParams, List<object> currentFilteredTargets, Dictionary<string, object> otherTargets) {
        /*---------------------------------------------------------------------------------
            idParams[0] is guaranteed to be a string with the name of the projectile in the
            Projectile folder if the user has entered the key correctly in the ability editor. 
        ---------------------------------------------------------------------------------*/
    }
}
