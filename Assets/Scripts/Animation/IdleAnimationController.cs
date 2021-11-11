using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnimationController : MonoBehaviour{

    private Animator _anim;
    private Camera _camera;
    [SerializeField]
    private int degree; //can delete this , just for debug
    [SerializeField]
    private int degreeVariation;
    void Awake(){
        degreeVariation = 10;
        _anim = gameObject.GetComponent<Animator>();
        _camera = Camera.main;
    }

    void Update(){
        degree = getDegree(MousePosition(_camera)); //can delete this , just for debug
        _anim.Play(getDegree(MousePosition(_camera))+"_PlayerIdle"); 

    }
    private Vector3 MousePosition(Camera _camera){
        return _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,_camera.transform.position.z));
    }

    private int getDegree(Vector3 mousePosition){
        
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        float anglefixed = angle < 0 ? (360.0f + angle) : angle;
        anglefixed = Mathf.Round((anglefixed/degreeVariation)) * degreeVariation;
        anglefixed = anglefixed < 360 ? anglefixed : 0;

        return (int)anglefixed;
    }
}
