using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnimationController : MonoBehaviour
{
    EventProcessor unitEventHandler;
    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private AnimationClip[] _animIdleClip;
    private AnimationClip OriginalClip ; // hardcode
    private AnimatorOverrideController aoc;
    private Camera _camera;
    private int degreeClipLength;
    [SerializeField]
    private int degreeVariation;

    private void Awake()
    {
        //unitEventHandler = GetComponent<UnitEventManager>().UnitEventHandler;
        degreeClipLength = 36;
        degreeVariation = 10;        
        _anim = gameObject.GetComponent<Animator>();
        _camera = Camera.main;
        _animIdleClip = new AnimationClip[degreeClipLength];

        loadResources();

        OriginalClip = _animIdleClip[0];// hardcode
        aoc = new AnimatorOverrideController(_anim.runtimeAnimatorController);
        _anim.runtimeAnimatorController = aoc;
    }
    private void Start()
    {
        unitEventHandler = GetComponent<UnitEventManager>().UnitEventHandler;// move to here
        unitEventHandler.StartListening("OnMoveOrderIssued", OnMoveOrderIssued);
    }

    private void OnMoveOrderIssued(object destination)
    {
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(OriginalClip, _animIdleClip[getDegree((Vector3)(destination)) / 10]));// every clip 10 degree 
        aoc.ApplyOverrides(anims);
    }

    private int getDegree(Vector3 mousePosition)
    {
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        float anglefixed = angle < 0 ? (360.0f + angle) : angle;
        anglefixed = Mathf.Round((anglefixed/degreeVariation)) * degreeVariation;
        anglefixed = anglefixed < 360 ? anglefixed : 0;

        return (int)anglefixed;
    }
    private void loadResources()
    {
        for(int i = 0 ; i < degreeClipLength ; i++)
            _animIdleClip[i] = (AnimationClip)Resources.Load("Animations/Idle animation clip/"+ i * 10 +"_PlayerIdle");
        
    }
}
