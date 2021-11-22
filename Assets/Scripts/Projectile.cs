using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 50.0f;
    private Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        enabled = false;
        Destroy(gameObject);
    }
}
