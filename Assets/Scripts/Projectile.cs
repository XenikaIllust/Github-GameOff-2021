using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 50.0f;
    private Rigidbody2D rigidbody2D;
    private Alliance launchingUnitAlliance;
    private float damage;
    private float timeToLive;

    // Start is called before the first frame update
    public void Launch()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = transform.right * speed;
    }

    public void Init(Alliance launchingUnitAlliance, float damage, float timeToLive) {
        this.launchingUnitAlliance = launchingUnitAlliance;
        this.damage = damage;
        this.timeToLive = timeToLive;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Unit targetUnit = other.GetComponentInParent<Unit>();

        if(targetUnit != null && targetUnit.alliance != launchingUnitAlliance) {
            targetUnit.unitEventHandler.RaiseEvent("OnDamageTaken", damage);
            Destroy(gameObject);
        }
    }

    IEnumerator StartLifeTimer(float timeToLive) {
        yield return new WaitForSeconds(timeToLive);
        Destroy(gameObject);
    }
}
