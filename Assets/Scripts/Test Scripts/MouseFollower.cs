using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    Camera camera;
    private void Awake() {
        camera = FindObjectOfType<Camera>();
    }

   // Update is called once per frame
    // void Update()
    // {
    //     transform.position = Input.mousePosition + Vector3.one * 100;
    // }

    IEnumerator DestroyAfterCountdown() {
        yield return new WaitForSeconds(20);
        Destroy(gameObject);
    }
}
