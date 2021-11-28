using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/Game Action/Fire Projectile")]
public class FireProjectileAction : GameActionBlock
{
    public override void Invoke(float timeToLive, AbilityStatsDict abilityStatsDict, string[] idParams, List<object> currentFilteredTargets,
        Dictionary<string, object> otherTargets)
    {
        /*---------------------------------------------------------------------------------
            idParams[0] is guaranteed to be a string with the name of the projectile in the
            Projectile folder if the user has entered the key correctly in the ability editor. 

            idParams[1] is guaranteed to be a key of the projectile's damage stat
        ---------------------------------------------------------------------------------*/

        Unit firingUnit = (Unit)currentFilteredTargets[0];
        Projectile projectilePrefab = Resources.Load<Projectile>("Projectiles/" + idParams[0]);

        Projectile projectile = Instantiate<Projectile>(projectilePrefab, firingUnit.transform.position, firingUnit.PseudoObject.transform.rotation);
        projectile.Init(firingUnit.alliance, abilityStatsDict[idParams[1]], timeToLive);
    }
}