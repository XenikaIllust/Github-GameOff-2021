using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnimationController2 : MonoBehaviour{
    EventProcessor unitEventHandler;
    private Animator _anim;
    private Camera _camera;
    //[SerializeField]
    //private int degree; //can delete this , just for debug
    [SerializeField]
    private int degreeVariation;
    private void Awake(){
        //unitEventHandler = GetComponent<UnitEventManager>().UnitEventHandler;
        degreeVariation = 10;
        _anim = gameObject.GetComponent<Animator>();
        _camera = Camera.main;
    }
    private void Start()
    {
        unitEventHandler = GetComponent<UnitEventManager>().UnitEventHandler;// move to here
        
        unitEventHandler.StartListening("OnMoveOrderIssued", OnMoveOrderIssued);
    }

    private void OnMoveOrderIssued(object destination)
    {
        _anim.Play(getDegree((Vector3)(destination))+"_PlayerIdle"); 
    }

    private void OnStopOrderIssued(object arg0)
    {
    }
    /*

    void Update(){
        degree = getDegree(MousePosition(_camera)); //can delete this , just for debug
        _anim.Play(getDegree(MousePosition(_camera))+"_PlayerIdle"); 

    }
    private Vector3 MousePosition(Camera _camera){
        return _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,_camera.transform.position.z));
    }
    */
    private int getDegree(Vector3 mousePosition){
        
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        float anglefixed = angle < 0 ? (360.0f + angle) : angle;
        anglefixed = Mathf.Round((anglefixed/degreeVariation)) * degreeVariation;
        anglefixed = anglefixed < 360 ? anglefixed : 0;

        return (int)anglefixed;
    }
}
