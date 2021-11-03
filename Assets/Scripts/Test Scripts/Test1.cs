using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Test1 : MonoBehaviour
{
    private UnityAction<object> someListener;
    public UnityEvent HitFloorEvent;

    private void Awake()
    {
        someListener = new UnityAction<object>((object o) => Debug.Log(o));
    }

    private void OnCollisionEnter(Collision collision)
    {
        EventManager.RaiseEvent("test1", collision.gameObject.name);

        if (collision.gameObject.name == "Floor")
        {
            HitFloorEvent.Invoke();
        }
    }

    private void OnEnable()
    {
        EventManager.StartListening("test1", someListener);
    }

    private void OnDisable()
    {
        EventManager.StopListening("test1", someListener);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("test1", someListener);
    }
}
