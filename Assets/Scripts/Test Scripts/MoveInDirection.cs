using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInDirection : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] Vector3 move_direction;
    [SerializeField] float velocity = 2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb != null)
        {
            rb.transform.Translate(move_direction * velocity * Time.deltaTime);
        }
    }
}
