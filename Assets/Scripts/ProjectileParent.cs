using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileParent : MonoBehaviour
{
    [SerializeField] private float speed = 50.0f;
    private float timeToLive;

    public List<Projectile> projectiles = new List<Projectile>();

    public void Init(Alliance launchingUnitAlliance, float damage, float timeToLive) {
        timeToLive = timeToLive + 1; // let the parent die after the children projectiles have died

        foreach(Projectile projectile in projectiles) {
            projectile.Init(launchingUnitAlliance, damage, timeToLive);
            projectile.Launch();
        }
    }

    IEnumerator StartLifeTimer(float timeToLive) {
        yield return new WaitForSeconds(timeToLive);
        Destroy(gameObject);
    }
}
