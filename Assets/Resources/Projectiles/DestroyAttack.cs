using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAttack : MonoBehaviour
{
    public int timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += 1;

        if (timer >= 400)
        {
            Destroy(gameObject);
        }
    }
}
