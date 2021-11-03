using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastFromMouseClick : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit info;

            if (Physics.Raycast(ray, out info))
            {
            }
        }
    }
}
